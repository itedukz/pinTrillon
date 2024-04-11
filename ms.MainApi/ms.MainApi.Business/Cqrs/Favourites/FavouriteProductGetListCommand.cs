using AutoMapper;
using MediatR;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Favourites;
using ms.MainApi.DataAccess.Products;
using ms.MainApi.Entity.Models.DbModels.Favourites;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.Dtos.Products;
using ms.MainApi.Entity.Models.Dtos.Responses;

namespace ms.MainApi.Business.Cqrs.Favourites;

public class FavouriteProductGetListCommand : IRequest<IMainResponseDto>
{
    public class Handler : IRequestHandler<FavouriteProductGetListCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IFavouriteProductDal _favouriteDal;
        private readonly IProductDal _productDal;
        private readonly IAuthInformationRepository _authInformationRepository;

        public Handler(IMapper mapper, IFavouriteProductDal favouriteDal, IProductDal productDal, 
            IAuthInformationRepository authInformationRepository)
        {
            _mapper = mapper;
            _favouriteDal = favouriteDal;
            _productDal = productDal;
            _authInformationRepository = authInformationRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(FavouriteProductGetListCommand request, CancellationToken cancellationToken)
        {
            int userId = _authInformationRepository.GetUserId();
            if (userId == 0)
                return new MainResponseDto("User not found");

            List<FavouriteProduct> entities = await _favouriteDal.GetAllAsync(i => i.userId == userId);
            if(entities == null || entities.Count == 0)
                return new MainResponseDto("Favourite not found");


            List<int> productsId = entities.Select(e => e.productId).ToList();
            List<Product> products = await _productDal.GetAllAsync(i => productsId.Contains(i.id));
            List<ProductShortDto> productsDto = products.Select(i => _mapper.Map<ProductShortDto>(i)).ToList();

            return new MainResponseDto(productsDto);
        }
    }
}