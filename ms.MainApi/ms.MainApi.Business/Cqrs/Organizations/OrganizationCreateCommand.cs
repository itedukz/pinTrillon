using AutoMapper;
using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Organizations;
using ms.MainApi.Entity.Models.DbModels.Organizations;
using ms.MainApi.Entity.Models.Dtos.Organizations;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Organizations;

public class OrganizationCreateCommand : IRequest<IMainResponseDto>
{
    public OrganizationCreateDto form { get; }

    public OrganizationCreateCommand(OrganizationCreateDto _form)
    {
        form = _form;
    }

    public class Handler : IRequestHandler<OrganizationCreateCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMediator _mediator;
        private readonly IMessagesRepository _messagesRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<OrganizationCreateCommand> _validator;
        private readonly IOrganizationDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IAuthInformationRepository _authInformationRepository;

        public Handler(IMediator mediator, IMessagesRepository messagesRepository, IMapper mapper, IValidator<OrganizationCreateCommand> validator,
            IOrganizationDal entityDal, IPermissionCheck checkPermission, IAuthInformationRepository authInformationRepository)
        {
            _mediator = mediator;
            _messagesRepository = messagesRepository;
            _mapper = mapper;
            _validator = validator;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _authInformationRepository = authInformationRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(OrganizationCreateCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.create
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.organization, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            Organization entity = _mapper.Map<Organization>(request.form);
            entity.userId = _authInformationRepository.GetUserId();
            await _entityDal.AddAsync(entity);

            OrganizationDto? controlDto = await _mediator.Send(new getOrganizationCommand(entity.id));

            return new MainResponseDto(controlDto, permission.permittedActions);
        }
    }
}