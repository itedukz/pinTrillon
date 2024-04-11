using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.Entity.Models.DbModels.Identity;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Identities.Permissions;

public class PermissionDeleteCommand : IRequest<IMainResponseDto>
{
    public int roleId { get; }
    public int permissionId { get; }

    public PermissionDeleteCommand(int _roleId, int _permissionId)
    {
        roleId = _roleId;
        permissionId = _permissionId;
    }

    public class Handler : IRequestHandler<PermissionDeleteCommand, IMainResponseDto>
    {
        #region DI
        private readonly IPermissionDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<PermissionDeleteCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IPermissionDal entityDal, IPermissionCheck checkPermission,
            IValidator<PermissionDeleteCommand> validator, IMessagesRepository messagesRepository)
        {
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(PermissionDeleteCommand request, CancellationToken cancellationToken)
        {
            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.delete, PermissionAction.update
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.permission, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            Permission? permissionForDelete = await _entityDal.GetAsync(i => i.roleId == request.roleId && i.permissionId == request.permissionId);
            if (permissionForDelete != null)
                await _entityDal.DeleteAsync(permissionForDelete);
                        
            return new MainResponseDto("Permission has been deleted");
        }
    }
}