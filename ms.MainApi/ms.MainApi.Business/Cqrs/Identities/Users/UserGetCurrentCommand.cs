using AutoMapper;
using MediatR;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.Entity.Models.DbModels.Identity;
using ms.MainApi.Entity.Models.Dtos.Identities.Users;
using ms.MainApi.Entity.Models.Dtos.Responses;

namespace ms.MainApi.Business.Cqrs.Identities.Users;

public class UserGetCurrentCommand : IRequest<IMainResponseDto>
{
    public class Handler : IRequestHandler<UserGetCurrentCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IUserDal _entityDal;
        private readonly IAuthInformationRepository _authInformationRepository;

        public Handler(IMapper mapper, IUserDal entityDal, IAuthInformationRepository authInformationRepository)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _authInformationRepository = authInformationRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(UserGetCurrentCommand request, CancellationToken cancellationToken)
        {
            User? entity = await _entityDal.GetAsync(i => i.id == _authInformationRepository.GetUserId());

            return new MainResponseDto(_mapper.Map<UserDto>(entity));
        }
    }
}