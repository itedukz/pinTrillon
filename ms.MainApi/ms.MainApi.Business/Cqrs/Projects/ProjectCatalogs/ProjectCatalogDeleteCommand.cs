using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Projects;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Projects.ProjectCatalogs;

public class ProjectCatalogDeleteCommand : IRequest<IMainResponseDto>
{
    public int id { get; }

    public ProjectCatalogDeleteCommand(int _id)
    {
        id = _id;
    }

    public class Handler : IRequestHandler<ProjectCatalogDeleteCommand, IMainResponseDto>
    {
        #region DI
        private readonly IProjectCatalogDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<ProjectCatalogDeleteCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IProjectCatalogDal entityDal, IPermissionCheck checkPermission,
            IValidator<ProjectCatalogDeleteCommand> validator, IMessagesRepository messagesRepository)
        {
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(ProjectCatalogDeleteCommand request, CancellationToken cancellationToken)
        {
            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.delete
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.projectCatalog, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            await _entityDal.DeleteAsync(i => i.id == request.id);

            return new MainResponseDto("Entity has been deleted");
        }
    }
}