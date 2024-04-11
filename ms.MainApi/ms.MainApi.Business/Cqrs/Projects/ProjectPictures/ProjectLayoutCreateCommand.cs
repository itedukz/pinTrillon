using MediatR;
using Microsoft.AspNetCore.Http;
using ms.MainApi.Business.Services;
using ms.MainApi.DataAccess.Projects;
using ms.MainApi.Entity.Models.DbModels.Projects;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Projects.ProjectPictures;

public class ProjectLayoutCreateCommand : IRequest<IMainResponseDto>
{
    public int projectId { get; }
    IFormFile? file { get; }

    public ProjectLayoutCreateCommand(int _projectId, IFormFile? _file)
    {
        projectId = _projectId;
        file = _file;
    }

    public class Handler : IRequestHandler<ProjectLayoutCreateCommand, IMainResponseDto>
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

        public async Task<IMainResponseDto> Handle(ProjectLayoutCreateCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.create, PermissionAction.update, PermissionAction.delete,
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.project, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            if (request.file == null || request.file.Length == 0)
                return new MainResponseDto("ProjectLayout file is null");
            try
            {
                CheckFolder(request.projectId.ToString());
                string path = "files/project/layouts/" + request.projectId.ToString() + "/";
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.file.FileName).ToLower();
                var saveFilePath = Path.Combine(fullPath, fileName);

                await using var stream = new FileStream(saveFilePath, FileMode.Create);
                await request.file.CopyToAsync(stream, cancellationToken);

                
                ProjectLayout entity = new ProjectLayout
                {
                    projectId = request.projectId,
                    FileName = fileName,
                    FilePath = "/" + path + fileName,
                };
                await _entityDal.AddAsync(entity);

                return new MainResponseDto("ProjectLayouts is saved");
            }
            catch (Exception ex)
            {
                return new MainResponseDto(ex.Message);
            }
        }

        private void CheckFolder(string path)
        {
            var directoryPathList = new List<string>()
            {
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files"),
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "project"),
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "project", "layouts"),
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "project", "layouts", path)
            };
            foreach (var item in directoryPathList)
                if (!Directory.Exists(item))
                    Directory.CreateDirectory(item);
        }
    }
}