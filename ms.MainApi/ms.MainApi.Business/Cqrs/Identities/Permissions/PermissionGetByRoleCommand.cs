using AutoMapper;
using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.Entity.Models.DbModels.Identity;
using ms.MainApi.Entity.Models.Dtos.Identities.Permissions;
using ms.MainApi.Entity.Models.Dtos.Identities.Roles;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Identities.Permissions;

public class PermissionGetByRoleCommand : IRequest<IMainResponseDto>
{
    public int roleId { get; }

    public PermissionGetByRoleCommand(int _roleId)
    {
        roleId = _roleId;
    }

    public class Handler : IRequestHandler<PermissionGetByRoleCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IPermissionCheck _checkPermission;
        private readonly IPermissionDal _permissionDal;
        private readonly IRoleDal _roleDal;
        private readonly IValidator<PermissionGetByRoleCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IMapper mapper, IPermissionCheck checkPermission, IPermissionDal permissionDal, IRoleDal roleDal,
            IValidator<PermissionGetByRoleCommand> validator, IMessagesRepository messagesRepository)
        {
            _mapper = mapper;
            _checkPermission = checkPermission;
            _permissionDal = permissionDal;
            _roleDal = roleDal;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(PermissionGetByRoleCommand request, CancellationToken cancellationToken)
        {
            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.getAll
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.permission, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            
            RolePermissionDto rolePermissions = new RolePermissionDto {
                role = _mapper.Map<RoleDto>(await _roleDal.GetAsync(i => i.id == request.roleId)),
                permissions = new List<PermissionActionDto>()
            };

            var permissions = await _permissionDal.GetAllAsync(i => i.roleId == request.roleId);
            foreach (Permission item in permissions.OrderBy(o => o.permissionId))
            {
                PermissionActionDto rolePermission = new PermissionActionDto
                { 
                    permission = PermissionConvert.toBaseClass(item.permissionId),
                    actions = AllowedActionsCrud(item.actions)
                };

                rolePermissions.permissions.Add(rolePermission);
            }

            return new MainResponseDto(rolePermissions, permission.permittedActions);
        }


        private List<PermissionActionDtlDto> AllowedActionsCrud(List<int>? allowedActions)
        {
            List<PermissionActionDtlDto> list = new List<PermissionActionDtlDto>();

            list.Add(new PermissionActionDtlDto { isAllowed = allowedActions != null && allowedActions.Contains((int)PermissionAction.getAll), id = (int)PermissionAction.getAll, name = PermissionAction.getAll.ToString(), column = 0, group = "get" });
            list.Add(new PermissionActionDtlDto { isAllowed = allowedActions != null && allowedActions.Contains((int)PermissionAction.getOwn), id = (int)PermissionAction.getOwn, name = PermissionAction.getOwn.ToString(), column = 1, group = "get" });
            list.Add(new PermissionActionDtlDto { isAllowed = allowedActions != null && allowedActions.Contains((int)PermissionAction.create), id = (int)PermissionAction.create, name = PermissionAction.create.ToString(), column = 0, group = "create" });
            list.Add(new PermissionActionDtlDto { isAllowed = allowedActions != null && allowedActions.Contains((int)PermissionAction.update), id = (int)PermissionAction.update, name = PermissionAction.update.ToString(), column = 0, group = "update" });
            list.Add(new PermissionActionDtlDto { isAllowed = allowedActions != null && allowedActions.Contains((int)PermissionAction.delete), id = (int)PermissionAction.delete, name = PermissionAction.delete.ToString(), column = 0, group = "delete" });

            return list;
        }
    }
}