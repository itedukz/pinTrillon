using AutoMapper;
using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.Entity.Models.DbModels.Identity;
using ms.MainApi.Entity.Models.Dtos.Identities.Users;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Identities.UserRoles;

public class UserRoleGetUsersCommand : IRequest<IMainResponseDto>
{
    public int roleId { get; }

    public UserRoleGetUsersCommand(int _roleId)
    {
        roleId = _roleId;
    }

    public class Handler : IRequestHandler<UserRoleGetUsersCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IPermissionCheck _checkPermission;
        private readonly IUserRoleDal _userRoleDal;
        private readonly IUserDal _userDal;
        private readonly IValidator<UserRoleGetUsersCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IMapper mapper, IPermissionCheck checkPermission, IUserRoleDal userRoleDal, IUserDal userDal,
            IValidator<UserRoleGetUsersCommand> validator, IMessagesRepository messagesRepository)
        {
            _mapper = mapper;
            _checkPermission = checkPermission;
            _userRoleDal = userRoleDal;
            _userDal = userDal;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(UserRoleGetUsersCommand request, CancellationToken cancellationToken)
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

            var roleUsers = await _userRoleDal.GetAllAsync(i => i.roleId == request.roleId);
            var bindUsers = roleUsers.Select(i => i.userId).ToList();

            List<User> sources = await _userDal.GetAllAsync(i => bindUsers.Contains(i.id));

            return new MainResponseDto(sources.Select(i => _mapper.Map<UserDto>(i)).ToList(), permission.permittedActions);
        }
    }
}