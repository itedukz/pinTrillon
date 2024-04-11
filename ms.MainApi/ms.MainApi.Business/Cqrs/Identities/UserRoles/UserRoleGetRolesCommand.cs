using AutoMapper;
using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.Entity.Models.DbModels.Identity;
using ms.MainApi.Entity.Models.Dtos.Identities.Roles;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Identities.UserRoles;

public class UserRoleGetRolesCommand : IRequest<IMainResponseDto>
{
    public int userId { get; }

    public UserRoleGetRolesCommand(int _userId)
    {
        userId = _userId;
    }

    public class Handler : IRequestHandler<UserRoleGetRolesCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IPermissionCheck _checkPermission;
        private readonly IUserRoleDal _userRoleDal;
        private readonly IRoleDal _roleDal;
        private readonly IValidator<UserRoleGetRolesCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IMapper mapper, IPermissionCheck checkPermission, IUserRoleDal userRoleDal,IRoleDal roleDal,
            IValidator<UserRoleGetRolesCommand> validator, IMessagesRepository messagesRepository)
        {
            _mapper = mapper;
            _checkPermission = checkPermission;
            _userRoleDal = userRoleDal;
            _roleDal = roleDal;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(UserRoleGetRolesCommand request, CancellationToken cancellationToken)
        {
            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.getAll,
                PermissionAction.getOwn
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.userRoleBind, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            var userRoles = await _userRoleDal.GetAllAsync(i => i.userId == request.userId);
            var bindRoles = userRoles.Select(i => i.roleId).ToList();

            List<Role> sources = await _roleDal.GetAllAsync(i => bindRoles.Contains(i.id));

            return new MainResponseDto(sources.Select(i => _mapper.Map<RoleDto>(i)).ToList(), permission.permittedActions);
        }
    }
}