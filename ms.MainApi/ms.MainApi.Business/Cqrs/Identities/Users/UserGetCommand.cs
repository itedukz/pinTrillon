using AutoMapper;
using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.Entity.Models.DbModels.Identity;
using ms.MainApi.Entity.Models.Dtos.Identities.Users;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Identities.Users;

public class UserGetCommand : IRequest<IMainResponseDto>
{
    public int id { get; }

    public UserGetCommand(int _id)
    {
        id = _id;
    }

    public class Handler : IRequestHandler<UserGetCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IUserDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IAuthInformationRepository _authInformationRepository;
        private readonly IValidator<UserGetCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IMapper mapper, IUserDal entityDal, IPermissionCheck checkPermission, IAuthInformationRepository authInformationRepository,
            IValidator<UserGetCommand> validator, IMessagesRepository messagesRepository)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _authInformationRepository = authInformationRepository;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(UserGetCommand request, CancellationToken cancellationToken)
        {
            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.getAll,
                PermissionAction.getOwn
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.user, actions);

            if (!permission.isSuccess || request.id != _authInformationRepository.GetUserId())
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            User? entity = await _entityDal.GetAsync(i => i.id == request.id);
            
            return new MainResponseDto(_mapper.Map<UserDto>(entity), permission.permittedActions);
        }
    }
}