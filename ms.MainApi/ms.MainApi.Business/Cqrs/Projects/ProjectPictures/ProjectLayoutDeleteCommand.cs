using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.DataAccess.Projects;
using ms.MainApi.Entity.Models.DbModels.Projects;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Projects.ProjectPictures;

public class ProjectLayoutDeleteCommand : IRequest<IMainResponseDto>
{
    public int projectId { get; }

    public ProjectLayoutDeleteCommand(int _projectId)
    {
        projectId = _projectId;
    }

    public class Handler : IRequestHandler<ProjectLayoutDeleteCommand, IMainResponseDto>
    {
        #region DI
        private readonly IProjectLayoutDal _entityDal;
        private readonly IPermissionCheck _checkPermission;

        public Handler(IProjectLayoutDal entityDal, IPermissionCheck checkPermission)
        {
            _entityDal = entityDal;
            _checkPermission = checkPermission;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(ProjectLayoutDeleteCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.create, PermissionAction.update, PermissionAction.delete,
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.project, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            ProjectLayout? entity = await _entityDal.GetAsync(i => i.projectId == request.projectId);
            if (entity == null)
                return new MainResponseDto("ProjectLayout not found", permission.permittedActions);

            try
            {
                string filePath = $"wwwroot{entity.FilePath}";
                var file = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                File.Delete(file);

                await _entityDal.DeleteAsync(entity);
            }
            catch { }

            return new MainResponseDto("ProjectLayout is deleted");
        }
    }
}