using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using ms.MainApi.Entity.Models.Services;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Identities.Users;

public class UserResetPasswordCommand : IRequest<IMainResponseDto>
{
    public int id { get; }

    public UserResetPasswordCommand(int _form)
    {
        id = _form;
    }

    public class Handler : IRequestHandler<UserResetPasswordCommand, IMainResponseDto>
    {
        #region DI
        private readonly IUserDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<UserResetPasswordCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IUserDal entityDal, IPermissionCheck checkPermission,
            IValidator<UserResetPasswordCommand> validator, IMessagesRepository messagesRepository)
        {
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(UserResetPasswordCommand request, CancellationToken cancellationToken)
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

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            var entity = await _entityDal.GetAsync(i => i.id == request.id);
            if (entity == null)
                return new MainResponseDto("Password has not been deleted");

            entity.passwordHash = HashMD5.HashMD5String("123456");
            await _entityDal.UpdateAsync(entity);

            return new MainResponseDto("Password has been reseted");
        }
    }
}