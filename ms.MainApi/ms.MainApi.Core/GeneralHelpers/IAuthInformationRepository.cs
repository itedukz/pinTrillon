using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ms.MainApi.Core.GeneralHelpers;

public interface IAuthInformationRepository
{
    int GetUserId();
}

public class AuthInformationRepository : IAuthInformationRepository
{
    #region DI
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthInformationRepository(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    #endregion

    public int GetUserId()
    {
        try
        {
            var context = _httpContextAccessor.HttpContext?.User;
            var identity = context?.Identities?.FirstOrDefault();
            if (identity == null) 
                return 0;

            var claims = identity.Claims.ToList();
            if(!claims.Any())
                return 0;

            string userId = claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value!;
            int user_id = 0;
            if (Int32.TryParse(userId, out user_id))
                return user_id;
        }
        catch { }

        return 0;
    }
}
