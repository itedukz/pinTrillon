using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Identities.Users;

public class UserDeleteCommand : IRequest<IMainResponseDto>
{
    public int id { get; }

    public UserDeleteCommand(int _id)
    {
        id = _id;
    }

    public class Handler : IRequestHandler<UserDeleteCommand, IMainResponseDto>
    {
        #region DI
        private readonly IUserDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<UserDeleteCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IUserDal entityDal, IPermissionCheck checkPermission,
            IValidator<UserDeleteCommand> validator, IMessagesRepository messagesRepository)
        {
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
        {
            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.delete
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.user, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            await _entityDal.DeleteAsync(i => i.id == request.id);

            return  new MainResponseDto("Entity has been deleted");
        }
    }
}