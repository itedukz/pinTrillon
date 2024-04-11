using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.DataAccess.Organizations;
using ms.MainApi.Entity.Models.DbModels.Organizations;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Organizations.OrganizationPictures;

public class OrganizationAvatarDeleteCommand : IRequest<IMainResponseDto>
{
    public int organizationId { get; }

    public OrganizationAvatarDeleteCommand(int _organizationId)
    {
        organizationId = _organizationId;
    }

    public class Handler : IRequestHandler<OrganizationAvatarDeleteCommand, IMainResponseDto>
    {
        #region DI
        private readonly IOrganizationDal _entityDal;
        private readonly IPermissionCheck _checkPermission;

        public Handler(IOrganizationDal entityDal, IPermissionCheck checkPermission)
        {
            _entityDal = entityDal;
            _checkPermission = checkPermission;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(OrganizationAvatarDeleteCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.create, PermissionAction.update, PermissionAction.delete,
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.organization, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            Organization? entity = await _entityDal.GetAsync(i => i.id == request.organizationId);
            if (entity == null)
                return new MainResponseDto("Organization avatar not found", permission.permittedActions);

            try
            {
                string filePath = $"wwwroot{entity.logoFilePath}";
                var file = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                File.Delete(file);

                entity.logoFileName = "";
                entity.logoFilePath = "";
                
                await _entityDal.UpdateAsync(entity);
            }
            catch { }

            return new MainResponseDto("Organization avatar is deleted");
        }
    }
}