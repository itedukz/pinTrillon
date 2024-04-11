using AutoMapper;
using MediatR;
using ms.MainApi.Business.ExpressionParser;
using ms.MainApi.DataAccess.Products;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.Dtos.Pictures;
using ms.MainApi.Entity.Models.Dtos.Products;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using ms.MainApi.Entity.Models.Pages.SearchPages;

namespace ms.MainApi.Business.Cqrs.MainPages;

public class SearchPageProductGetListCommand : IRequest<IMainResponseDto>
{
    public SearchProduct Form { get; }

    public SearchPageProductGetListCommand(SearchProduct form)
    {
        Form = form;
    }

    public class Handler : IRequestHandler<SearchPageProductGetListCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IProductDal _entityDal;
        private readonly IProductPictureDal _productPictureDal;
        private readonly ISearchProductExpressionParser _expressionParser;

        public Handler(IMapper mapper, IProductDal entityDal, IProductPictureDal productPictureDal,
            ISearchProductExpressionParser expressionParser)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _productPictureDal = productPictureDal;
            _expressionParser = expressionParser;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(SearchPageProductGetListCommand request, CancellationToken cancellationToken)
        {
            var filter = _expressionParser.ParseExpressionOf<Product>(request.Form);

            List<Product> products = await _entityDal.GetAllAsync(filter);

            if (request.Form.query!.materialsId != null && request.Form.query!.materialsId.Count > 0 && request.Form.query!.materialsId[0] != 0)
            {
                try
                {
                    products = products.Where(i => i.materialsId.All(m => request.Form.query!.materialsId.Contains(m))).ToList();
                    //products = query.ToList();
                }
                catch { }
            }

            if (request.Form.query!.colorsId != null && request.Form.query!.colorsId.Count > 0 && request.Form.query!.colorsId[0] != 0)
            {
                try
                {
                    products = products.Where(i => i.colorsId.All(m => request.Form.query!.colorsId.Contains(m))).ToList();
                    //products = query.ToList();
                }
                catch { }
            }

            int totalItems = products.Count;
            int pageSize = request.Form.pageSize > 0 ? request.Form.pageSize : 10;
            int page = request.Form.page > 0 ? request.Form.page - 1 : 0;
            products = products.Skip(page * pageSize).Take(pageSize).ToList();

            #region Sort
            //if (false && request.Form.sort != null)
            //{
            //    if (request.Form.sort.orderByName != null)
            //    {
            //        bool orderByName = request.Form.sort.orderByName ?? true;

            //        products = orderByName ?
            //            products.OrderBy(o => o.name).ToList() :
            //            products.OrderByDescending(o => o.name).ToList();
            //    }
            //    else if (request.Form.sort.orderByPrice != null)
            //    {
            //        bool orderByPrice = request.Form.sort.orderByPrice ?? true;

            //        products = orderByPrice ?
            //            products.OrderBy(o => o.price).ToList() :
            //            products.OrderByDescending(o => o.price).ToList();
            //    }
            //    else if (request.Form.sort.orderByDate != null)
            //    {
            //        bool orderByDate = request.Form.sort.orderByDate ?? true;

            //        products = orderByDate ?
            //            products.OrderBy(o => o.createdAt).ToList() :
            //            products.OrderByDescending(o => o.createdAt).ToList();
            //    }
            //}
            #endregion

            #region products with main pictures

            SearchProductPage result = new SearchProductPage
            {
                products = products.Select(i => _mapper.Map<ProductShortDto>(i)).ToList()
            };
            //result.products = products.Select(i => _mapper.Map<ProductShortDto>(i)).ToList();

            List<int> productsId = products.Select(i => i.id).ToList();
            List<ProductPicture> pictures = await _productPictureDal.GetAllAsync(i => productsId.Contains(i.productId) && i.isMain);

            foreach (ProductShortDto product in result.products)
            {
                ProductPicture? picture = pictures.FirstOrDefault(i => i.productId == product.id);
                product.picture = _mapper.Map<PictureDto>(picture);
            }
            #endregion

            #region Sort
            result.products = request.Form.sort switch
            {
                (int)SortEnum.orderByName => result.products.OrderBy(o => o.name).ToList(),
                (int)SortEnum.orderByNameDesc => result.products.OrderByDescending(o => o.name).ToList(),
                (int)SortEnum.orderByPrice => result.products.OrderBy(o => o.price).ToList(),
                (int)SortEnum.orderByPriceDesc => result.products.OrderByDescending(o => o.price).ToList(),
                (int)SortEnum.orderByDate => result.products.OrderBy(o => o.id).ToList(),
                (int)SortEnum.orderByDateDesc => result.products.OrderByDescending(o => o.id).ToList(),
                _ => result.products.OrderBy(o => o.name).ToList()
            };
            #endregion

            return new MainResponseDto(result, totalItems, page, pageSize);
        }
    }
}