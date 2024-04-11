using AutoMapper;
using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Products;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.Dtos.Products.Brands;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Products.Brands;

public class BrandGetCommand : IRequest<IMainResponseDto>
{
    public int id { get; }

    public BrandGetCommand(int _id)
    {
        id = _id;
    }

    public class Handler : IRequestHandler<BrandGetCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IBrandDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<BrandGetCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IMapper mapper, IBrandDal entityDal, IPermissionCheck checkPermission,
            IValidator<BrandGetCommand> validator, IMessagesRepository messagesRepository)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(BrandGetCommand request, CancellationToken cancellationToken)
        {
            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            #region checkPermission
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.productSettings);
            #endregion

            Brand? entity = await _entityDal.GetAsync(i => i.id == request.id);

            return new MainResponseDto(_mapper.Map<BrandDto>(entity), permission.permittedActions);
        }
    }
}