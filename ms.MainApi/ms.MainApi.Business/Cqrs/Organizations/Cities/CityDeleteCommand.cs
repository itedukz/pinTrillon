using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Organizations;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Organizations.Cities;

public class CityDeleteCommand : IRequest<IMainResponseDto>
{
    public int id { get; }

    public CityDeleteCommand(int _id)
    {
        id = _id;
    }

    public class Handler : IRequestHandler<CityDeleteCommand, IMainResponseDto>
    {
        #region DI
        private readonly ICityDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<CityDeleteCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(ICityDal entityDal, IPermissionCheck checkPermission,
            IValidator<CityDeleteCommand> validator, IMessagesRepository messagesRepository)
        {
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(CityDeleteCommand request, CancellationToken cancellationToken)
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
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.city, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            await _entityDal.DeleteAsync(i => i.id == request.id);

            return new MainResponseDto("Entity has been deleted");
        }
    }
}