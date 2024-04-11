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

namespace ms.MainApi.Business.Cqrs.Identities.Roles;

public class RoleCreateCommand : IRequest<IMainResponseDto>
{
    public RoleCreateDto form { get; }

    public RoleCreateCommand(RoleCreateDto _form)
    {
        form = _form;
    }

    public class Handler : IRequestHandler<RoleCreateCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IRoleDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<RoleCreateCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IMapper mapper, IRoleDal entityDal, IPermissionCheck checkPermission,
            IValidator<RoleCreateCommand> validator, IMessagesRepository messagesRepository)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
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

            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.role, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            Role entity = _mapper.Map<Role>(request.form);
            await _entityDal.AddAsync(entity);

            return new MainResponseDto(_mapper.Map<RoleDto>(entity), permission.permittedActions);
        }
    }
}