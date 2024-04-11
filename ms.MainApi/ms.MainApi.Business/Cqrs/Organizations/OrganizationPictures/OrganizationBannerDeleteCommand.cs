using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.DataAccess.Organizations;
using ms.MainApi.Entity.Models.DbModels.Organizations;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Organizations.OrganizationPictures;

public class OrganizationBannerDeleteCommand : IRequest<IMainResponseDto>
{
    public int organizationId { get; }

    public OrganizationBannerDeleteCommand(int _organizationId)
    {
        organizationId = _organizationId;
    }

    public class Handler : IRequestHandler<OrganizationBannerDeleteCommand, IMainResponseDto>
    {
        #region DI
        private readonly IOrganizationPictureDal _entityDal;
        private readonly IPermissionCheck _checkPermission;

        public Handler(IOrganizationPictureDal entityDal, IPermissionCheck checkPermission)
        {
            _entityDal = entityDal;
            _checkPermission = checkPermission;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(OrganizationBannerDeleteCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.create, PermissionAction.update, PermissionAction.delete,
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.organization, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            OrganizationPicture? entity = await _entityDal.GetAsync(i => i.organizationId == request.organizationId);
            if (entity == null)
                return new MainResponseDto("Organization Banner not found", permission.permittedActions);

            try
            {
                string filePath = $"wwwroot{entity.FilePath}";
                var file = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                File.Delete(file);

                await _entityDal.DeleteAsync(entity);
            }
            catch { }

            return new MainResponseDto("Organization Banner is deleted");
        }
    }
}