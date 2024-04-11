using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Organizations;
using ms.MainApi.Entity.Models.DbModels.Organizations;
using ms.MainApi.Entity.Models.Dtos.Pictures;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Organizations.OrganizationPictures;

public class OrganizationAvatarCreateCommand : IRequest<IMainResponseDto>
{
    public int organizationId { get; }
    IFormFile? file { get; }

    public OrganizationAvatarCreateCommand(int _organizationId, IFormFile? _file)
    {
        organizationId = _organizationId;
        file = _file;
    }

    public class Handler : IRequestHandler<OrganizationAvatarCreateCommand, IMainResponseDto>
    {
        #region DI
        private readonly IOrganizationDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<OrganizationAvatarCreateCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IOrganizationDal entityDal, IPermissionCheck checkPermission, 
            IValidator<OrganizationAvatarCreateCommand> validator, IMessagesRepository messagesRepository)
        {
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(OrganizationAvatarCreateCommand request, CancellationToken cancellationToken)
        {
            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.create, PermissionAction.update, PermissionAction.delete,
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.organization, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            if (request.file == null || request.file.Length == 0)
                return new MainResponseDto("Organization avatar file is null");

            Organization? entity = await _entityDal.GetAsync(i => i.id == request.organizationId);
            if (entity == null)
                return new MainResponseDto("Organization not found");

            try
            {
                CheckFolder(request.organizationId.ToString());
                string path = "files/organization/avatar/" + request.organizationId.ToString() + "/";
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.file.FileName).ToLower();
                var saveFilePath = Path.Combine(fullPath, fileName);

                await using var stream = new FileStream(saveFilePath, FileMode.Create);
                await request.file.CopyToAsync(stream, cancellationToken);

                entity.logoFileName = fileName;
                entity.logoFilePath = "/" + path + fileName;
                await _entityDal.UpdateAsync(entity);

                PictureShortDto pictureDto = new PictureShortDto
                {
                    FileName = entity.logoFileName,
                    FilePath = entity.logoFilePath
                };

                return new MainResponseDto(pictureDto, "Organization avatar is saved");
                //new MainResponseDto("Organization avatar is saved");
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
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "organization"),
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "organization", "avatar"),
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "organization", "avatar", path)
            };
            foreach (var item in directoryPathList)
                if (!Directory.Exists(item))
                    Directory.CreateDirectory(item);
        }
    }
}