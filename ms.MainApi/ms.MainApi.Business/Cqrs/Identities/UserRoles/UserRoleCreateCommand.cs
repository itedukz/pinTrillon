using AutoMapper;
using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.Entity.Models.DbModels.Identity;
using ms.MainApi.Entity.Models.Dtos.Identities.Roles;
using ms.MainApi.Entity.Models.Dtos.Identities.UserRoles;
using ms.MainApi.Entity.Models.Dtos.Identities.Users;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Identities.UserRoles;

public class UserRoleCreateCommand : IRequest<IMainResponseDto>
{
    public UserRoleCreateDto form { get; }

    public UserRoleCreateCommand(UserRoleCreateDto _form)
    {
        form = _form;
    }

    public class Handler : IRequestHandler<UserRoleCreateCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IUserRoleDal _userRoleDal;
        private readonly IUserDal _userDal;
        private readonly IRoleDal _roleDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<UserRoleCreateCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IMapper mapper, IPermissionCheck checkPermission,
            IUserRoleDal userRoleDal, IUserDal userDal, IRoleDal roleDal,
            IValidator<UserRoleCreateCommand> validator, IMessagesRepository messagesRepository)
        {
            _mapper = mapper;
            _checkPermission = checkPermission;
            _userRoleDal = userRoleDal;
            _userDal = userDal;
            _roleDal = roleDal;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(UserRoleCreateCommand request, CancellationToken cancellationToken)
        {
            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.create, PermissionAction.update
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.userRoleBind, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            var userRoles = await _userRoleDal.GetAllAsync(i => i.userId == request.form.userId);
            var RoleIds = userRoles.Select(i => i.roleId).ToList();

            // Include New RoleId to DB
            var userRolesForInsert = request.form.rolesId.Where(i => !RoleIds.Contains(i)).ToList();
            foreach (int roleId in userRolesForInsert)
            {
                UserRole insertUserRole = new UserRole
                {

                    userId = request.form.userId,
                    roleId = roleId
                };
                _userRoleDal.Add(insertUserRole);
            }

            var lastUserRoles = await _userRoleDal.GetAllAsync(i => i.userId == request.form.userId);
            var roles = await _roleDal.GetAllAsync(i => lastUserRoles.Select(p => p.roleId).Contains(i.id));

            UserRolesDto result = new UserRolesDto
            {
                user = _mapper.Map<UserDto>(await _userDal.GetAsync(i => i.id == request.form.userId)),
                roles = roles.Select(i => _mapper.Map<RoleShortDto>(i)).ToList()
            };

            return new MainResponseDto(result, permission.permittedActions);
        }
    }
}