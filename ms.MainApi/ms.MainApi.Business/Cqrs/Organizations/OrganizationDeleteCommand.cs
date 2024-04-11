using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Organizations;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Organizations;

public class OrganizationDeleteCommand : IRequest<IMainResponseDto>
{
    public int id { get; }

    public OrganizationDeleteCommand(int _id)
    {
        id = _id;
    }

    public class Handler : IRequestHandler<OrganizationDeleteCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMessagesRepository _messagesRepository;
        private readonly IValidator<OrganizationDeleteCommand> _validator;
        private readonly IOrganizationDal _entityDal;
        private readonly IPermissionCheck _checkPermission;

        public Handler(IMessagesRepository messagesRepository, IValidator<OrganizationDeleteCommand> validator,
            IOrganizationDal entityDal, IPermissionCheck checkPermission)
        {
            _messagesRepository = messagesRepository;
            _validator = validator;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(OrganizationDeleteCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.delete
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

            await _entityDal.DeleteAsync(i => i.id == request.id);

            return new MainResponseDto("Entity has been deleted");
        }
    }
}