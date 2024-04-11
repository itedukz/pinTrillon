using AutoMapper;
using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Products;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.Dtos.Products.Materials;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Products.Materials;

public class MaterialCreateCommand : IRequest<IMainResponseDto>
{
    public MaterialCreateDto form { get; }

    public MaterialCreateCommand(MaterialCreateDto _form)
    {
        form = _form;
    }

    public class Handler : IRequestHandler<MaterialCreateCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IMaterialDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<MaterialCreateCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IMapper mapper, IMaterialDal entityDal, IPermissionCheck checkPermission,
            IValidator<MaterialCreateCommand> validator, IMessagesRepository messagesRepository)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(MaterialCreateCommand request, CancellationToken cancellationToken)
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
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.productSettings, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            Material entity = _mapper.Map<Material>(request.form);
            await _entityDal.AddAsync(entity);

            return new MainResponseDto(_mapper.Map<MaterialDto>(entity), permission.permittedActions);
        }
    }
}