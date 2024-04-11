using AutoMapper;
using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Projects;
using ms.MainApi.Entity.Models.DbModels.Projects;
using ms.MainApi.Entity.Models.Dtos.Projects.ProjectCatalogs;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Projects.ProjectCatalogs;

public class ProjectCatalogUpdateCommand : IRequest<IMainResponseDto>
{
    public ProjectCatalogUpdateDto form { get; }

    public ProjectCatalogUpdateCommand(ProjectCatalogUpdateDto _form)
    {
        form = _form;
    }

    public class Handler : IRequestHandler<ProjectCatalogUpdateCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IProjectCatalogDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<ProjectCatalogUpdateCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IMapper mapper, IProjectCatalogDal entityDal, IPermissionCheck checkPermission,
            IValidator<ProjectCatalogUpdateCommand> validator, IMessagesRepository messagesRepository)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(ProjectCatalogUpdateCommand request, CancellationToken cancellationToken)
        {
            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.update
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.projectCatalog, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            ProjectCatalog entity = _mapper.Map<ProjectCatalog>(request.form);
            await _entityDal.UpdateAsync(entity);

            return new MainResponseDto(_mapper.Map<ProjectCatalogDto>(entity), permission.permittedActions);
        }
    }
}