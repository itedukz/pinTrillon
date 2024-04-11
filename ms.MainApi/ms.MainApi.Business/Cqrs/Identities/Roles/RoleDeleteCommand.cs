using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Identities.Roles;

public class RoleDeleteCommand : IRequest<IMainResponseDto>
{
    public int id { get; }

    public RoleDeleteCommand(int _id)
    {
        id = _id;
    }

    public class Handler : IRequestHandler<RoleDeleteCommand, IMainResponseDto>
    {
        #region DI
        private readonly IRoleDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<RoleDeleteCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IRoleDal entityDal, IPermissionCheck checkPermission,
            IValidator<RoleDeleteCommand> validator, IMessagesRepository messagesRepository)
        {
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(RoleDeleteCommand request, CancellationToken cancellationToken)
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
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.role, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            await _entityDal.DeleteAsync(i => i.id == request.id);

            return  new MainResponseDto("Entity has been deleted");
        }
    }
}