using AngleSharp.Dom;
using AutoMapper;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.Interfaces;
using ms.MainApi.DataAccess.Products;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.Dtos.Pictures;
using ms.MainApi.Entity.Models.Dtos.Products;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using ms.MainApi.Entity.Models.Services;
using Newtonsoft.Json;
using System.Linq;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Products;

public class ProductGetListCommand : IRequest<IMainResponseDto>
{
    public QueryPagination Form { get; }

    public ProductGetListCommand(QueryPagination form)
    {
        Form = form;
    }

    public class Handler : IRequestHandler<ProductGetListCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IProductDal _entityDal;
        private readonly IProductPictureDal _productPictureDal;
        private readonly IPermissionCheck _checkPermission;

        public Handler(IMapper mapper, IProductDal entityDal, IProductPictureDal productPictureDal, IPermissionCheck checkPermission)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _productPictureDal = productPictureDal;
            _checkPermission = checkPermission;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(ProductGetListCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.getAll,
                PermissionAction.getOwn
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.product, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            string Query = JsonConvert.SerializeObject(request.Form.query);
            List<Product> sources = await _entityDal.GetAllQueryAsync(Query);

            if (!string.IsNullOrEmpty(request.Form.search))
            {
                string srchTxt = request.Form.search.ToLower();

                sources = await _entityDal.GetAllAsync(i => i.name.ToLower().Contains(srchTxt) ||
                                                            i.description!.ToLower().Contains(srchTxt));
            }

            int totalItems = sources.Count;
            int pageSize = request.Form.pageSize > 0 ? request.Form.pageSize : 10;
            int page = request.Form.page > 0 ? request.Form.page - 1 : 0;
            sources = sources.Skip(page * pageSize).Take(pageSize).ToList();

            List<ProductPicture> pictures = await _productPictureDal.GetAllAsync();

            List<ProductDto> sourcesDto = new List<ProductDto>();
            foreach(Product source in sources)
            {
                ProductDto sourceDto = _mapper.Map<ProductDto>(source);
                List<ProductPicture> sourcePictures = pictures.Where(i => i.productId == source.id).OrderBy(o => o.isMain).Take(2).ToList();
                sourceDto.pictures = sourcePictures.Select(i => _mapper.Map<PictureDto>(i)).ToList();

                sourcesDto.Add(sourceDto);
            }

            return new MainResponseDto(sourcesDto, permission.permittedActions, totalItems, page, pageSize);
        }
    }
}