using AutoMapper;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.Entity.Models.DbModels.Identity;
using ms.MainApi.Entity.Models.Dtos.Identities.Roles;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using ms.MainApi.Entity.Models.Services;
using Newtonsoft.Json;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Identities.Roles;

public class RoleGetListCommand : IRequest<IMainResponseDto>
{
    public QueryPagination Form { get; }

    public RoleGetListCommand(QueryPagination form)
    {
        Form = form;
    }

    public class Handler : IRequestHandler<RoleGetListCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IRoleDal _entityDal;
        private readonly IPermissionCheck _checkPermission;

        public Handler(IMapper mapper, IRoleDal entityDal, IPermissionCheck checkPermission)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(RoleGetListCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.getAll,
                PermissionAction.getOwn
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.role, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            int totalItems = 0;
            int pageSize = request.Form.pageSize > 0 ? request.Form.pageSize : 10;
            int page = request.Form.page > 0 ? request.Form.page - 1 : 0;

            List<Role> sources = new List<Role>();
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

            return new MainResponseDto(sources.Select(i => _mapper.Map<RoleDto>(i)).ToList(), permission.permittedActions, totalItems, page, pageSize);
        }
    }
}