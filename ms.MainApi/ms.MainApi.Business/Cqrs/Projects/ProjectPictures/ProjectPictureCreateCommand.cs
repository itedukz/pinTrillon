using MediatR;
using Microsoft.AspNetCore.Http;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers.RabbitMq;
using ms.MainApi.Core.GeneralHelpers.RabbitMq.Models;
using ms.MainApi.DataAccess.Projects;
using ms.MainApi.Entity.Models.DbModels.Projects;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using ms.MainApi.Entity.Models.RabbitMq;
using Newtonsoft.Json;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Projects.ProjectPictures;

public class ProjectPictureCreateCommand : IRequest<IMainResponseDto>
{
    public int projectId { get; }
    IFormFile? file { get; }
    public bool isMain { get; }

    public ProjectPictureCreateCommand(int _projectId, IFormFile? _file, bool _isMain)
    {
        projectId = _projectId;
        file = _file;
        isMain = _isMain;
    }

    public class Handler : IRequestHandler<ProjectPictureCreateCommand, IMainResponseDto>
    {
        #region DI
        private readonly IRabbitMqHelperRepository _rabbitMqHelperRepository;
        private readonly IProjectPictureDal _entityDal;
        private readonly IPermissionCheck _checkPermission;

        public Handler(IRabbitMqHelperRepository rabbitMqHelperRepository, IProjectPictureDal entityDal,
            IPermissionCheck checkPermission)
        {
            _rabbitMqHelperRepository = rabbitMqHelperRepository;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(ProjectPictureCreateCommand request, CancellationToken cancellationToken)
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
                return new MainResponseDto("ProjectPicture file is null");
            try
            {
                CheckFolder(request.projectId.ToString());
                string path = "files/project/pictures/" + request.projectId.ToString() + "/";
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.file.FileName).ToLower();
                var saveFilePath = Path.Combine(fullPath, fileName);

                await using var stream = new FileStream(saveFilePath, FileMode.Create);
                await request.file.CopyToAsync(stream, cancellationToken);

                List<ProjectPicture>? entities = await _entityDal.GetAllAsync(i => i.projectId == request.projectId && i.isMain && request.isMain);
                foreach (ProjectPicture item in entities)
                {
                    item.isMain = false;
                    await _entityDal.UpdateAsync(item);
                }

                ProjectPicture entity = new ProjectPicture
                {
                    projectId = request.projectId,
                    FileName = fileName,
                    FilePath = "/" + path + fileName,
                    isMain = request.isMain,
                    picture = new byte[0]
                };

                using (var memoryStream = new MemoryStream())
                {
                    request.file.CopyTo(memoryStream);
                    entity.picture = memoryStream.ToArray();
                }

                await _entityDal.AddAsync(entity);

                try
                {
                    _rabbitMqHelperRepository.CreateMessage(new RabbitMqSendRequestModel()
                    {
                        QueueInformation = new RabbitMqQueueInformationModel()
                        {
                            QueueName = "project-picture-sender-bg-service"
                        },
                        Message = JsonConvert.SerializeObject(new MessageNotificationDto
                        {
                            referenceId = entity.id,
                            MessageType = MessageTypes.projectPicture
                        })
                    });
                }
                catch
                {
                    return new MainResponseDto("ProjectPictures is saved with RabbitMq Producer Error");
                }

                return new MainResponseDto("ProjectPictures is saved");
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
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "project", "pictures"),
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "project", "pictures", path)
            };
            foreach (var item in directoryPathList)
                if (!Directory.Exists(item))
                    Directory.CreateDirectory(item);
        }
    }
}