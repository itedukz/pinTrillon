using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.DataAccess.Projects;
using ms.MainApi.Entity.Models.DbModels.Projects;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Projects.ProjectPictures;

public class ProjectPictureDeleteCommand : IRequest<IMainResponseDto>
{
    public int projectId { get; }
    public int pictureId { get; }

    public ProjectPictureDeleteCommand(int _projectId, int _pictureId)
    {
        projectId = _projectId;
        pictureId = _pictureId;
    }

    public class Handler : IRequestHandler<ProjectPictureDeleteCommand, IMainResponseDto>
    {
        #region DI
        private readonly IProjectPictureDal _entityDal;
        private readonly IPermissionCheck _checkPermission;

        public Handler(IProjectPictureDal entityDal, IPermissionCheck checkPermission)
        {
            _entityDal = entityDal;
            _checkPermission = checkPermission;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(ProjectPictureDeleteCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.create, PermissionAction.update, PermissionAction.delete,
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.project, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            ProjectPicture? entity = await _entityDal.GetAsync(i => i.id == request.pictureId && i.projectId == request.projectId);
            if (entity == null)
                return new MainResponseDto("ProjectPicture not found", permission.permittedActions);

            try
            {
                string filePath = $"wwwroot{entity.FilePath}";
                var file = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                File.Delete(file);

                await _entityDal.DeleteAsync(entity);
            }
            catch { }

            return new MainResponseDto("ProjectPicture is deleted");
        }
    }
}