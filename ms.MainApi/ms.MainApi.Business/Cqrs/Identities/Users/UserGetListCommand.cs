using AutoMapper;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.Entity.Models.DbModels.Identity;
using ms.MainApi.Entity.Models.Dtos.Identities.Roles;
using ms.MainApi.Entity.Models.Dtos.Identities.Users;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using ms.MainApi.Entity.Models.Services;
using Newtonsoft.Json;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Identities.Users;

public class UserGetListCommand : IRequest<IMainResponseDto>
{
    public QueryPagination Form { get; }

    public UserGetListCommand(QueryPagination form)
    {
        Form = form;
    }

    public class Handler : IRequestHandler<UserGetListCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IUserDal _entityDal;
        private readonly IRoleDal _roleDal;
        private readonly IUserRoleDal _userRoleDal;
        private readonly IPermissionCheck _checkPermission;

        public Handler(IMapper mapper, IUserDal entityDal, IRoleDal roleDal, IUserRoleDal userRoleDal,
            IPermissionCheck checkPermission)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _roleDal = roleDal;
            _userRoleDal = userRoleDal;
            _checkPermission = checkPermission;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(UserGetListCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.getAll,
                PermissionAction.getOwn
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.user, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            int totalItems = 0;

            List<User> sources = new List<User>();
            if (!string.IsNullOrEmpty(request.Form.search))
            {
                string srchTxt = request.Form.search.ToLower();

                sources = await _entityDal.GetAllAsync(i => i.firstName.ToLower().Contains(srchTxt) ||
                                                         i.lastName.ToLower().Contains(srchTxt) ||
                                                         i.middleName.ToLower().Contains(srchTxt) ||
                                                         i.email.ToLower().Contains(srchTxt) ||
                                                         i.phoneNumber.ToLower().Contains(srchTxt));
                totalItems = sources.Count;
                int pageSize = request.Form.pageSize > 0 ? request.Form.pageSize : 10;
                int page = request.Form.page > 0 ? request.Form.page - 1 : 0;

                sources = sources.Skip(page * pageSize).Take(pageSize).ToList();
            }
            else
            {
                string Query = JsonConvert.SerializeObject(request.Form.query);
                sources = _entityDal.GetAllQuery(ref totalItems, request.Form.page, request.Form.pageSize, Query);
            }

            var roles = await _roleDal.GetAllAsync();
            var usersRoles = await _userRoleDal.GetAllAsync();

            var usersDto = sources.Select(i => _mapper.Map<UserWithRolesDto>(i)).ToList();
            foreach (var userDto in usersDto)
            {
                var userRoles = usersRoles.Where(i => i.userId == userDto.id).ToList();
                var bindRoleIds = userRoles.Select(i => i.roleId).ToList();
                var role = roles.Where(i => bindRoleIds.Contains(i.id)).ToList();

                userDto.roles = role.Select(i => _mapper.Map<RoleShortDto>(i)).ToList();
            }
            
            return new MainResponseDto(usersDto, permission.permittedActions, totalItems, request.Form.page, request.Form.pageSize);
        }
    }
}