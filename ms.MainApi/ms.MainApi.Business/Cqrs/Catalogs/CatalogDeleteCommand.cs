using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Catalogs;

public class CatalogDeleteCommand : IRequest<IMainResponseDto>
{
    public int id { get; }

    public CatalogDeleteCommand(int _id)
    {
        id = _id;
    }

    public class Handler : IRequestHandler<CatalogDeleteCommand, IMainResponseDto>
    {
        #region DI
        private readonly ICatalogDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<CatalogDeleteCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(ICatalogDal entityDal, IPermissionCheck checkPermission,
            IValidator<CatalogDeleteCommand> validator, IMessagesRepository messagesRepository)
        {
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(CatalogDeleteCommand request, CancellationToken cancellationToken)
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
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.catalog, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            await _entityDal.DeleteAsync(i => i.id == request.id);

            return new MainResponseDto("Entity has been deleted");
        }
    }
}