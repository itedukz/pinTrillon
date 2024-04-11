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

public class ProjectCatalogGetCommand : IRequest<IMainResponseDto>
{
    public int id { get; }

    public ProjectCatalogGetCommand(int _id)
    {
        id = _id;
    }

    public class Handler : IRequestHandler<ProjectCatalogGetCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IProjectCatalogDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<ProjectCatalogGetCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IMapper mapper, IProjectCatalogDal entityDal, IPermissionCheck checkPermission,
            IValidator<ProjectCatalogGetCommand> validator, IMessagesRepository messagesRepository)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(ProjectCatalogGetCommand request, CancellationToken cancellationToken)
        {
            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.getAll,
                PermissionAction.getOwn
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.projectCatalog, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            ProjectCatalog? entity = await _entityDal.GetAsync(i => i.id == request.id);

            return new MainResponseDto(_mapper.Map<ProjectCatalogDto>(entity), permission.permittedActions);
        }
    }
}