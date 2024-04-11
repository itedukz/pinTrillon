using AutoMapper;
using MediatR;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Baskets;
using ms.MainApi.DataAccess.Products;
using ms.MainApi.Entity.Models.DbModels.Baskets;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.Dtos.Products;
using ms.MainApi.Entity.Models.Dtos.Responses;

namespace ms.MainApi.Business.Cqrs.Baskets;

public class BasketProductGetListCommand : IRequest<IMainResponseDto>
{
    public class Handler : IRequestHandler<BasketProductGetListCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IBasketProductDal _basketDal;
        private readonly IProductDal _productDal;
        private readonly IAuthInformationRepository _authInformationRepository;

        public Handler(IMapper mapper, IBasketProductDal basketDal, IProductDal productDal,
            IAuthInformationRepository authInformationRepository)
        {
            _mapper = mapper;
            _basketDal = basketDal;
            _productDal = productDal;
            _authInformationRepository = authInformationRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(BasketProductGetListCommand request, CancellationToken cancellationToken)
        {
            int userId = _authInformationRepository.GetUserId();
            if (userId == 0)
                return new MainResponseDto("User not found");

            List<BasketProduct> entities = await _basketDal.GetAllAsync(i => i.userId == userId);
            if (entities == null || entities.Count == 0)
                return new MainResponseDto("Basket not found");


            List<int> productsId = entities.Select(e => e.productId).ToList();
            List<Product> products = await _productDal.GetAllAsync(i => productsId.Contains(i.id));
            List<ProductShortDto> productsDto = products.Select(i => _mapper.Map<ProductShortDto>(i)).ToList();

            return new MainResponseDto(productsDto);
        }
    }
}