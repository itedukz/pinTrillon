using AutoMapper;
using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Projects;
using ms.MainApi.Entity.Models.DbModels.Projects;
using ms.MainApi.Entity.Models.Dtos.Projects;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Projects;

public class ProjectCreateCommand : IRequest<IMainResponseDto>
{
    public ProjectCreateDto form { get; }

    public ProjectCreateCommand(ProjectCreateDto _form)
    {
        form = _form;
    }

    public class Handler : IRequestHandler<ProjectCreateCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMediator _mediator;
        private readonly IMessagesRepository _messagesRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ProjectCreateCommand> _validator;
        private readonly IProjectDal _entityDal;
        private readonly IPermissionCheck _checkPermission;

        public Handler(IMediator mediator, IMessagesRepository messagesRepository, IMapper mapper, IValidator<ProjectCreateCommand> validator,
            IProjectDal entityDal, IPermissionCheck checkPermission)
        {
            _mediator = mediator;
            _messagesRepository = messagesRepository;
            _mapper = mapper;
            _validator = validator;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(ProjectCreateCommand request, CancellationToken cancellationToken)
        {
            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.create
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.project, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            Project entity = _mapper.Map<Project>(request.form);
            await _entityDal.AddAsync(entity);

            ProjectDto? entityDto = await _mediator.Send(new getProjectCommand(entity.id));

            return new MainResponseDto(entityDto, permission.permittedActions);
        }
    }
}