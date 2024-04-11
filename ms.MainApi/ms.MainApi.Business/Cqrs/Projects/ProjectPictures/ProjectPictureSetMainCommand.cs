using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.DataAccess.Projects;
using ms.MainApi.Entity.Models.DbModels.Projects;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Projects.ProjectPictures;

public class ProjectPictureSetMainCommand : IRequest<IMainResponseDto>
{
    public int pictureId { get; }

    public ProjectPictureSetMainCommand(int _pictureId)
    {
        pictureId = _pictureId;
    }

    public class Handler : IRequestHandler<ProjectPictureSetMainCommand, IMainResponseDto>
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

        public async Task<IMainResponseDto> Handle(ProjectPictureSetMainCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.create, PermissionAction.update, PermissionAction.delete,
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.project, actions);

            //if (!permission.isSuccess)
            //    return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            try
            {
                ProjectPicture? entity = await _entityDal.GetAsync(i => i.id == request.pictureId);
                if (entity == null)
                    return new MainResponseDto("ProjectPictures not found", HttpStatusCode.NotFound);

                List<ProjectPicture>? entities = await _entityDal.GetAllAsync(i => i.projectId == entity.projectId && i.isMain);
                foreach (ProjectPicture item in entities)
                {
                    item.isMain = false;
                    await _entityDal.UpdateAsync(item);
                }

                entity.isMain = true;
                await _entityDal.UpdateAsync(entity);

                return new MainResponseDto("ProjectPictures isMain saved");
            }
            catch (Exception ex)
            {
                return new MainResponseDto(ex.Message);
            }
        }
    }
}