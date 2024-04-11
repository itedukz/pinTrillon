using AutoMapper;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.DataAccess.Organizations;
using ms.MainApi.Entity.Models.DbModels.Organizations;
using ms.MainApi.Entity.Models.Dtos.Organizations;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using ms.MainApi.Entity.Models.Services;
using Newtonsoft.Json;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Organizations;

public class OrganizationGetListCommand : IRequest<IMainResponseDto>
{
    public QueryPagination Form { get; }

    public OrganizationGetListCommand(QueryPagination form)
    {
        Form = form;
    }

    public class Handler : IRequestHandler<OrganizationGetListCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IOrganizationDal _entityDal;
        private readonly IPermissionCheck _checkPermission;

        public Handler(IMapper mapper, IOrganizationDal entityDal, IPermissionCheck checkPermission)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(OrganizationGetListCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.getAll,
                PermissionAction.getOwn
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.organization, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            string Query = JsonConvert.SerializeObject(request.Form.query);
            List<Organization> sources = await _entityDal.GetAllQueryAsync(Query);

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

            return new MainResponseDto(sources.Select(i => _mapper.Map<OrganizationDto>(i)).ToList(), permission.permittedActions, totalItems, page, pageSize);
        }
    }
}