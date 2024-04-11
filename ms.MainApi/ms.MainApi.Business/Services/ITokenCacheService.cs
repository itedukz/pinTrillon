using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.Entity.Models.Dtos.Identities.Users;
using ms.MainApi.Entity.Models.Enums;
using ms.MainApi.Entity.Models.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ms.MainApi.Business.Services;

public interface ITokenCacheService
{
    public Token? RegisterUser(UserDto _user);
}

public class TokenCacheService : ITokenCacheService
{
    #region DI
    private object _lock = "1";
    private List<Token> Tokens { get; set; } = new List<Token>();
    private readonly IUserRoleDal _userRoleDal;
    private readonly IPermissionDal _permissionDal;

    public TokenCacheService(IUserRoleDal userRoleDal, IPermissionDal permissionDal)
    {
        _userRoleDal = userRoleDal;
        _permissionDal = permissionDal;
    }
    #endregion

    public Token? RegisterUser(UserDto _user)
    {
        try
        {
            return CreateOrRefreshToken(_user);
        }
        catch
        {
            return null;
        }
    }

    protected Token? CreateOrRefreshToken(UserDto user)
    {
        try
        {
            Token? userToken = Tokens.FirstOrDefault(t => t.User.email.ToLower() == user.email.ToLower());

            if (userToken != null)
            {
                userToken.ExpirationDate = DateTime.Now.AddDays(1);
                userToken.Value = CreateToken(user);
            }
            else
            {
                userToken = new()
                {
                    User = user,
                    ExpirationDate = DateTime.Now.AddDays(1),
                    Id = Guid.NewGuid().ToString(),
                    Value = CreateToken(user)
                };

                lock (_lock)
                {
                    Tokens.Add(userToken);
                }
            }

            return userToken;
        }
        catch
        {
            return null;
        }
    }

    protected string CreateToken(UserDto user)
    {
        try
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.email.ToLower().Trim()),
                new Claim(ClaimTypes.Name, user.firstName + " " + user.lastName),
                new Claim(ClaimTypes.GivenName, user.firstName),
                new Claim(ClaimTypes.Surname, user.lastName),
                // new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Email, user.email.ToLower().Trim())
            };

            var userRole = _userRoleDal.Get(i => i.userId == user.id);
            if (userRole != null)
            {
                var permissions = _permissionDal.GetAll(i => i.roleId == userRole.roleId);
                foreach (var item in permissions)
                {
                    var permissionName = (PermissionName)item.permissionId;
                    if (item.actions == null || !item.actions.Any()) continue;
                    foreach (var item2 in item.actions)
                    {
                        var action = (PermissionAction)item2;
                        // For example : asset_get
                        // And we will write to controller [Authorize(Roles="asset_get, asset_create")]
                        claims.Add(new Claim(ClaimTypes.Role, $"{permissionName.ToString()}_{action.ToString()}"));
                    }
                }
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            DateTime now = DateTime.UtcNow;
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AuthInformation");
            int LifeTimeHours = int.TryParse(configuration.GetSection("LifeTimeHours").Value, out LifeTimeHours)
                ? 24 : LifeTimeHours;

            // Create JWT-token value
            var jwt = new JwtSecurityToken(
                issuer: configuration.GetSection("Issuer").Value,
                audience: configuration.GetSection("Audience").Value,
                notBefore: now,
                claims: claimsIdentity.Claims,
                expires: now.AddHours(LifeTimeHours),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("SigningKey").Value!)),
                    SecurityAlgorithms.HmacSha256Signature)
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
        catch
        {
            return "";
        }
    }
}