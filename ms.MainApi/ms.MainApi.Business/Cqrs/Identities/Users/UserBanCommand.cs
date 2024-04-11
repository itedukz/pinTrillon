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

namespace ms.MainApi.Business.Cqrs.Identities.Users;

public class UserBanCommand : IRequest<IMainResponseDto>
{
    public UserBanDto form { get; }

    public UserBanCommand(UserBanDto _form)
    {
        form = _form;
    }

    public class Handler : IRequestHandler<UserBanCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IUserDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<UserBanCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IMapper mapper, IUserDal entityDal, IPermissionCheck checkPermission,
            IValidator<UserBanCommand> validator, IMessagesRepository messagesRepository)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(UserBanCommand request, CancellationToken cancellationToken)
        {
            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.update, PermissionAction.delete
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.user, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            User? entity = await _entityDal.GetAsync(i => i.id == request.form.userId);
            if (entity == null)
                return new MainResponseDto("User not found", HttpStatusCode.BadRequest);

            entity.isActive = request.form.isBan;
            await _entityDal.UpdateAsync(entity);

            return new MainResponseDto(_mapper.Map<UserDto>(entity), permission.permittedActions);
        }
    }
}