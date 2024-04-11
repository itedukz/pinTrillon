using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.Entity.Models.DbModels.Identity;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Identities.Users;

public class UserAvatarUploadCommand : IRequest<IMainResponseDto>
{
    public int? userId { get; }
    IFormFile? file { get; }

    public UserAvatarUploadCommand(int? _userId, IFormFile? _file)
    {
        userId = _userId;
        file = _file;
    }

    public class Handler : IRequestHandler<UserAvatarUploadCommand, IMainResponseDto>
    {
        #region DI
        private readonly IUserDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IAuthInformationRepository _authInformationRepository;

        public Handler(IUserDal entityDal, IPermissionCheck checkPermission, 
            IAuthInformationRepository authInformationRepository)
        {
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _authInformationRepository = authInformationRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(UserAvatarUploadCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.update
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.user, actions);

            if (!permission.isSuccess && request.userId != _authInformationRepository.GetUserId())
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            if (request.file == null || request.file.Length == 0)
                return new MainResponseDto("User avatar file is null");

            int userId = request.userId ?? _authInformationRepository.GetUserId();

            User? entity = await _entityDal.GetAsync(i => i.id == userId);
            if(entity == null)
                return new MainResponseDto("User not found");

            CheckFolder(entity.id.ToString());
            string path = "files/pictures/" + entity.id.ToString() + "/";
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.file.FileName).ToLower();
            var saveFilePath = Path.Combine(fullPath, fileName);

            await using var stream = new FileStream(saveFilePath, FileMode.Create);
            await request.file.CopyToAsync(stream, cancellationToken);

            entity.fileName = fileName;
            entity.filePath = "/" + path + fileName;

            await _entityDal.UpdateAsync(entity);

            return new MainResponseDto("User avatar is saved");
        }

        private void CheckFolder(string path)
        {
            var directoryPathList = new List<string>()
            {
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files"),
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "pictures"),
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "pictures", path)
            };
            foreach (var item in directoryPathList)
                if (!Directory.Exists(item))
                    Directory.CreateDirectory(item);
        }
    }
}