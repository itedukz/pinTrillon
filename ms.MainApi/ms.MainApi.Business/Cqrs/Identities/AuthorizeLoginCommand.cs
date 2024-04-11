using AutoMapper;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.Entity.Models.DbModels.Identity;
using ms.MainApi.Entity.Models.Dtos.Identities.Users;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Services;

namespace ms.MainApi.Business.Cqrs.Identities;

public class AuthorizeLoginCommand : IRequest<IMainResponseDto>
{
    public UserLoginDto form { get; }

    public AuthorizeLoginCommand(UserLoginDto _form)
    {
        form = _form;
    }

    public class Handler : IRequestHandler<AuthorizeLoginCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IUserDal _entityDal;
        private readonly ITokenCacheService _tokenCacheService;

        public Handler(IMapper mapper, IUserDal entityDal, ITokenCacheService tokenCacheService)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _tokenCacheService = tokenCacheService;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(AuthorizeLoginCommand request, CancellationToken cancellationToken)
        {
            User? entity = await _entityDal.GetAsync(i => i.email == request.form.email.ToLower().Trim() &&
                                                          i.passwordHash == HashMD5.HashMD5String(request.form.password));

            if(entity == null)
                return new MainResponseDto("user not found");
            else if(entity.isActive == false)
                return new MainResponseDto("user is not active");

            UserDto entityDto = _mapper.Map<UserDto>(entity);

            return new MainResponseDto(_tokenCacheService.RegisterUser(entityDto).Value, "Bearer token");
        }
    }
}