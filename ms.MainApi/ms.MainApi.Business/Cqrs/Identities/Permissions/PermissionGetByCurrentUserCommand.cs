using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Entity.Models.Dtos.Identities.Permissions;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;

namespace ms.MainApi.Business.Cqrs.Identities.Permissions;

public class PermissionGetByCurrentUserCommand : IRequest<IMainResponseDto>
{
    public class Handler : IRequestHandler<PermissionGetByCurrentUserCommand, IMainResponseDto>
    {
        #region DI
        private readonly IPermissionCheck _checkPermission;

        public Handler(IPermissionCheck checkPermission)
        {
            _checkPermission = checkPermission;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(PermissionGetByCurrentUserCommand request, CancellationToken cancellationToken)
        {            
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.permission);
            
            CurrentUserRolePermissionsDto? permissions = new CurrentUserRolePermissionsDto()
            {
                permissions = await _checkPermission.GetCurrentUserPermissions()
            };

            return new MainResponseDto(permissions, permission.permittedActions);
        }
    }
}