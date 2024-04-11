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

public class CatalogGetCommand : IRequest<IMainResponseDto>
{
    public int id { get; }

    public CatalogGetCommand(int _id)
    {
        id = _id;
    }

    public class Handler : IRequestHandler<CatalogGetCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly ICatalogDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<CatalogGetCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IMapper mapper, ICatalogDal entityDal, IPermissionCheck checkPermission,
            IValidator<CatalogGetCommand> validator, IMessagesRepository messagesRepository)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(CatalogGetCommand request, CancellationToken cancellationToken)
        {
            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            #region checkPermission
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.catalog);
            #endregion

            Catalog? entity = await _entityDal.GetAsync(i => i.id == request.id);

            return new MainResponseDto(_mapper.Map<CatalogDto>(entity), permission.permittedActions);
        }
    }
}