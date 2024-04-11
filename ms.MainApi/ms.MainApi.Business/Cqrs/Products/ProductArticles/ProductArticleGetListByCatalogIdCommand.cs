using AutoMapper;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.DataAccess;
using ms.MainApi.DataAccess.Products;
using ms.MainApi.Entity.Models.DbModels.Catalogs;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.Dtos.Catalogs;
using ms.MainApi.Entity.Models.Dtos.Products.ProductArticles;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;

namespace ms.MainApi.Business.Cqrs.Products.ProductArticles;

public class ProductArticleGetListByCatalogIdCommand : IRequest<IMainResponseDto>
{
    public int catalogId { get; }

    public ProductArticleGetListByCatalogIdCommand(int CatalogId)
    {
        catalogId = CatalogId;
    }

    public class Handler : IRequestHandler<ProductArticleGetListByCatalogIdCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IProductArticleDal _entityDal;
        private readonly ICatalogDal _catalogDal;
        private readonly IPermissionCheck _checkPermission;

        public Handler(IMapper mapper, IProductArticleDal entityDal, ICatalogDal catalogDal, IPermissionCheck checkPermission)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _catalogDal = catalogDal;
            _checkPermission = checkPermission;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(ProductArticleGetListByCatalogIdCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.productSettings);
            #endregion

            List<ProductArticle> sources = await _entityDal.GetAllAsync(i => i.catalogId == request.catalogId);

            Catalog catalog = await _catalogDal.GetAsync(i => i.id == request.catalogId);
            List<ProductArticleDto> sourcesDto = new List<ProductArticleDto>();
            foreach (ProductArticle source in sources)
            {
                ProductArticleDto sourceDto = _mapper.Map<ProductArticleDto>(source);
                sourceDto.catalog = _mapper.Map<CatalogDto>(catalog);

                sourcesDto.Add(sourceDto);
            }

            return new MainResponseDto(sourcesDto, permission.permittedActions);
        }
    }
}