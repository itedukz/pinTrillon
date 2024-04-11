using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Entity.Models.Dtos.Identities.Permissions;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Identities.Permissions;

public class PermissionGetNameListCommand : IRequest<IMainResponseDto>
{
    public class Handler : IRequestHandler<PermissionGetNameListCommand, IMainResponseDto>
    {
        #region DI
        private readonly IPermissionCheck _checkPermission;

        public Handler(IPermissionCheck checkPermission)
        {
            _checkPermission = checkPermission;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(PermissionGetNameListCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.getAll,
                PermissionAction.getOwn,
                PermissionAction.create,
                PermissionAction.update,
                PermissionAction.delete
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.permission, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            PermissionListDto permissionName = new PermissionListDto()
            {
                permissions = PermissionActionMethod.entityPermissions
            };

            return new MainResponseDto(permissionName, permission.permittedActions);
        }
    }
}