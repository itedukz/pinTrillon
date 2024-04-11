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
using ms.MainApi.Entity.Models.Services;
using Newtonsoft.Json;

namespace ms.MainApi.Business.Cqrs.Products.ProductArticles;

public class ProductArticleGetListCommand : IRequest<IMainResponseDto>
{
    public QueryPagination Form { get; }

    public ProductArticleGetListCommand(QueryPagination form)
    {
        Form = form;
    }

    public class Handler : IRequestHandler<ProductArticleGetListCommand, IMainResponseDto>
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

        public async Task<IMainResponseDto> Handle(ProductArticleGetListCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.productSettings);
            #endregion

            int totalItems = 0;
            int pageSize = request.Form.pageSize > 0 ? request.Form.pageSize : 10;
            int page = request.Form.page > 0 ? request.Form.page - 1 : 0;

            List<ProductArticle> sources = new List<ProductArticle>();
            if (!string.IsNullOrEmpty(request.Form.search))
            {
                string srchTxt = request.Form.search.ToLower();

                sources = await _entityDal.GetAllAsync(i => i.name.ToLower().Contains(srchTxt) ||
                                                            i.description.ToLower().Contains(srchTxt));
                totalItems = sources.Count;
                sources = sources.Skip(page * pageSize).Take(pageSize).ToList();
            }
            else
            {
                string Query = JsonConvert.SerializeObject(request.Form.query);
                sources = _entityDal.GetAllQuery(ref totalItems, page, pageSize, Query);
            }

            List<Catalog> catalogs = await _catalogDal.GetAllAsync();
            List<ProductArticleDto> sourcesDto = new List<ProductArticleDto>();
            foreach (ProductArticle source in sources)
            {
                ProductArticleDto sourceDto = _mapper.Map<ProductArticleDto>(source);
                sourceDto.catalog = _mapper.Map<CatalogDto>(catalogs.FirstOrDefault(i => i.id == source.catalogId));

                sourcesDto.Add(sourceDto);
            }

            return new MainResponseDto(sourcesDto, permission.permittedActions, totalItems, page, pageSize);
        }
    }
}