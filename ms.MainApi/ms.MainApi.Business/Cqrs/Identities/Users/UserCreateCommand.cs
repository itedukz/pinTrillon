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
using ms.MainApi.Entity.Models.Services;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Identities.Users;

public class UserCreateCommand : IRequest<IMainResponseDto>
{
    public UserCreateDto form { get; }

    public UserCreateCommand(UserCreateDto _form)
    {
        form = _form;
    }

    public class Handler : IRequestHandler<UserCreateCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IUserDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<UserCreateCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IMapper mapper, IUserDal entityDal, IPermissionCheck checkPermission, 
            IValidator<UserCreateCommand> validator, IMessagesRepository messagesRepository)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.create
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.user, actions);
            #endregion

            User entity = _mapper.Map<User>(request.form);
            entity.passwordHash = HashMD5.HashMD5String(request.form.password.Trim());
            entity.email = request.form.email.ToLower().Trim();
            entity.isActive = true;

            await _entityDal.AddAsync(entity);

            return new MainResponseDto(_mapper.Map<UserDto>(entity), permission.permittedActions);
        }
    }
}