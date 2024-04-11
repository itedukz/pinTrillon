using AutoMapper;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.DataAccess.Products;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.Dtos.Products.Brands;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using ms.MainApi.Entity.Models.Services;
using Newtonsoft.Json;

namespace ms.MainApi.Business.Cqrs.Products.Brands;

public class BrandGetListCommand : IRequest<IMainResponseDto>
{
    public QueryPagination Form { get; }

    public BrandGetListCommand(QueryPagination form)
    {
        Form = form;
    }

    public class Handler : IRequestHandler<BrandGetListCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IBrandDal _entityDal;
        private readonly IPermissionCheck _checkPermission;

        public Handler(IMapper mapper, IBrandDal entityDal, IPermissionCheck checkPermission)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(BrandGetListCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.productSettings);
            #endregion

            string Query = JsonConvert.SerializeObject(request.Form.query);
            List<Brand> sources = await _entityDal.GetAllQueryAsync(Query);

            if (!string.IsNullOrEmpty(request.Form.search))
            {
                string srchTxt = request.Form.search.ToLower();
                sources = await _entityDal.GetAllAsync(i => i.name.ToLower().Contains(srchTxt) ||
                                                            i.description!.ToLower().Contains(srchTxt));                
            }

            int totalItems = sources.Count; ;
            int pageSize = request.Form.pageSize > 0 ? request.Form.pageSize : 10;
            int page = request.Form.page > 0 ? request.Form.page - 1 : 0;
            sources = sources.Skip(page * pageSize).Take(pageSize).ToList();

            return new MainResponseDto(sources.Select(i => _mapper.Map<BrandDto>(i)).ToList(), permission.permittedActions, totalItems, page, pageSize);
        }
    }
}