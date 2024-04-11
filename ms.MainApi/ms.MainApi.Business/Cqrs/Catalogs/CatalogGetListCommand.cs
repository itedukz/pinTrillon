using AutoMapper;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.DataAccess;
using ms.MainApi.Entity.Models.DbModels.Catalogs;
using ms.MainApi.Entity.Models.Dtos.Catalogs;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using ms.MainApi.Entity.Models.Services;
using Newtonsoft.Json;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Catalogs;

public class CatalogGetListCommand : IRequest<IMainResponseDto>
{
    public QueryPagination Form { get; }

    public CatalogGetListCommand(QueryPagination form)
    {
        Form = form;
    }

    public class Handler : IRequestHandler<CatalogGetListCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly ICatalogDal _entityDal;
        private readonly IPermissionCheck _checkPermission;

        public Handler(IMapper mapper, ICatalogDal entityDal, IPermissionCheck checkPermission)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(CatalogGetListCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.catalog);
            #endregion

            string Query = JsonConvert.SerializeObject(request.Form.query);
            List<Catalog> sources = await _entityDal.GetAllQueryAsync(Query);

            if (!string.IsNullOrEmpty(request.Form.search))
            {
                string srchTxt = request.Form.search.ToLower();
                sources = await _entityDal.GetAllAsync(i => i.name.ToLower().Contains(srchTxt) ||
                                                            i.description.ToLower().Contains(srchTxt));
            }
            int totalItems = sources.Count;
            int pageSize = request.Form.pageSize > 0 ? request.Form.pageSize : 10;
            int page = request.Form.page > 0 ? request.Form.page - 1 : 0;
            sources = sources.Skip(page * pageSize).Take(pageSize).ToList();

            return new MainResponseDto(sources.Select(i => _mapper.Map<CatalogDto>(i)).ToList(), permission.permittedActions, totalItems, page, pageSize);
        }
    }
}