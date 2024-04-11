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

public class UserUpdateCommand : IRequest<IMainResponseDto>
{
    public UserUpdateDto form { get; }

    public UserUpdateCommand(UserUpdateDto _form)
    {
        form = _form;
    }

    public class Handler : IRequestHandler<UserUpdateCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IUserDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IAuthInformationRepository _authInformationRepository;
        private readonly IValidator<UserUpdateCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IMapper mapper,
            IUserDal entityDal, IPermissionCheck checkPermission, IAuthInformationRepository authInformationRepository,
            IValidator<UserUpdateCommand> validator, IMessagesRepository messagesRepository)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _authInformationRepository = authInformationRepository;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
        {
            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.update
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.user, actions);

            if (!permission.isSuccess || request.form.id != _authInformationRepository.GetUserId())
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            User? entity = await _entityDal.GetAsync(i => i.id == request.form.id);
            if(entity == null)
                return new MainResponseDto("User not found", permission.permittedActions);

            entity.firstName = request.form.firstName;
            entity.lastName = request.form.lastName;
            entity.middleName = request.form.middleName;
            entity.phoneNumber = request.form.phoneNumber;

            await _entityDal.UpdateAsync(entity);

            return new MainResponseDto(_mapper.Map<UserDto>(entity), permission.permittedActions);
        }
    }
}