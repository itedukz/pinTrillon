using AutoMapper;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.DataAccess.Products;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.Dtos.Products.Materials;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using ms.MainApi.Entity.Models.Services;
using Newtonsoft.Json;

namespace ms.MainApi.Business.Cqrs.Products.Materials;

public class MaterialGetListCommand : IRequest<IMainResponseDto>
{
    public QueryPagination Form { get; }

    public MaterialGetListCommand(QueryPagination form)
    {
        Form = form;
    }

    public class Handler : IRequestHandler<MaterialGetListCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IMaterialDal _entityDal;
        private readonly IPermissionCheck _checkPermission;

        public Handler(IMapper mapper, IMaterialDal entityDal, IPermissionCheck checkPermission)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(MaterialGetListCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.productSettings);
            #endregion

            int totalItems = 0;
            int pageSize = request.Form.pageSize > 0 ? request.Form.pageSize : 10;
            int page = request.Form.page > 0 ? request.Form.page - 1 : 0;

            List<Material> sources = new List<Material>();
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

            return new MainResponseDto(sources.Select(i => _mapper.Map<MaterialDto>(i)).ToList(), permission.permittedActions, totalItems, page, pageSize);
        }
    }
}