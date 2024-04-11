using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Models.Dtos.Organizations;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Organizations;

public class OrganizationGetCommand : IRequest<IMainResponseDto>
{
    public int id { get; }

    public OrganizationGetCommand(int _id)
    {
        id = _id;
    }

    public class Handler : IRequestHandler<OrganizationGetCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMediator _mediator;
        private readonly IMessagesRepository _messagesRepository;
        private readonly IValidator<OrganizationGetCommand> _validator;
        private readonly IPermissionCheck _checkPermission;

        public Handler(IMediator mediator, IMessagesRepository messagesRepository, IValidator<OrganizationGetCommand> validator,
            IPermissionCheck checkPermission)
        {
            _mediator = mediator;
            _messagesRepository = messagesRepository;
            _validator = validator;
            _checkPermission = checkPermission;

        }
        #endregion

        public async Task<IMainResponseDto> Handle(OrganizationGetCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.getAll,
                PermissionAction.getOwn
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

            OrganizationDto? entityDto = await _mediator.Send(new getOrganizationCommand(request.id));

            return new MainResponseDto(entityDto, permission.permittedActions);
        }
    }
}