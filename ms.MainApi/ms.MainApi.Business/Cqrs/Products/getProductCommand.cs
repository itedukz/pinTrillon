using AutoMapper;
using MediatR;
using ms.MainApi.DataAccess;
using ms.MainApi.DataAccess.Products;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.Dtos.Catalogs;
using ms.MainApi.Entity.Models.Dtos.Pictures;
using ms.MainApi.Entity.Models.Dtos.Products;
using ms.MainApi.Entity.Models.Dtos.Products.Brands;
using ms.MainApi.Entity.Models.Dtos.Products.Materials;
using ms.MainApi.Entity.Models.Dtos.Products.ProductArticles;
using ms.MainApi.Entity.Models.Enums;

namespace ms.MainApi.Business.Cqrs.Products;

public class getProductCommand : IRequest<ProductDto?>
{
    public int id { get; }

    public getProductCommand(int _id)
    {
        id = _id;
    }

    public class Handler : IRequestHandler<getProductCommand, ProductDto?>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IProductDal _entityDal;
        private readonly IProductArticleDal _productArticleDal;
        private readonly IBrandDal _brandDal;
        private readonly IMaterialDal _materialDal;
        private readonly IProductPictureDal _productPictureDal;
        private readonly ICatalogDal _catalogDal;

        public Handler(IMapper mapper, IProductDal entityDal, IProductArticleDal productArticleDal, IBrandDal brandDal, 
            IMaterialDal materialDal, IProductPictureDal productPictureDal, ICatalogDal catalogDal)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _productArticleDal = productArticleDal;
            _brandDal = brandDal;
            _materialDal = materialDal;
            _productPictureDal = productPictureDal;
            _catalogDal = catalogDal;
        }
        #endregion

        public async Task<ProductDto?> Handle(getProductCommand request, CancellationToken cancellationToken)
        {
            Product? entity = await _entityDal.GetAsync(i => i.id == request.id);
            if (entity == null)
                return null;

            ProductDto entityDto = _mapper.Map<ProductDto>(entity);
            entityDto.catalog = _mapper.Map<CatalogDto>(await _catalogDal.GetAsync(i => i.id == entity.catalogId));
            entityDto.measure = new Entity.Models.Dtos.Measures.MeasureDto(entity.height, entity.width, entity.length, entity.measureType);
            entityDto.productArticle = _mapper.Map<ProductArticleDto>(await _productArticleDal.GetAsync(i => i.id == entity.productArticleId));
            entityDto.brand = _mapper.Map<BrandDto>(await _brandDal.GetAsync(i => i.id == entity.brandId));
            entityDto.colors = ColorEnumMethod.getByList(entity.colorsId);

            List<Material> materials = await _materialDal.GetAllAsync(i => entity.materialsId.Contains(i.id));
            entityDto.materials = materials.Select(i => _mapper.Map<MaterialDto>(i)).ToList();

            List<ProductPicture> pictures = await _productPictureDal.GetAllAsync(i => i.productId == entity.id);
            entityDto.pictures = pictures.Select(i => _mapper.Map<PictureDto>(i)).ToList();

            return entityDto;
        }
    }
}