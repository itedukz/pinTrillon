using AutoMapper;
using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess;
using ms.MainApi.Entity.Models.DbModels.Catalogs;
using ms.MainApi.Entity.Models.Dtos.Catalogs;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Catalogs;

public class CatalogUpdateCommand : IRequest<IMainResponseDto>
{
    public CatalogDto form { get; }

    public CatalogUpdateCommand(CatalogDto _form)
    {
        form = _form;
    }

    public class Handler : IRequestHandler<CatalogUpdateCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly ICatalogDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<CatalogUpdateCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IMapper mapper, ICatalogDal entityDal, IPermissionCheck checkPermission,
            IValidator<CatalogUpdateCommand> validator, IMessagesRepository messagesRepository)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(CatalogUpdateCommand request, CancellationToken cancellationToken)
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
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.catalog, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            Catalog entity = _mapper.Map<Catalog>(request.form);
            await _entityDal.UpdateAsync(entity);

            return new MainResponseDto(_mapper.Map<CatalogDto>(entity), permission.permittedActions);
        }
    }
}