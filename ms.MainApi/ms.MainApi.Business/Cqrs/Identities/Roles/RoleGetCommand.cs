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

public class RoleGetCommand : IRequest<IMainResponseDto>
{
    public int id { get; }

    public RoleGetCommand(int _id)
    {
        id = _id;
    }

    public class Handler : IRequestHandler<RoleGetCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IRoleDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<RoleGetCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IMapper mapper, IRoleDal entityDal, IPermissionCheck checkPermission,
            IValidator<RoleGetCommand> validator, IMessagesRepository messagesRepository)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(RoleGetCommand request, CancellationToken cancellationToken)
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
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.role, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            Role? entity = await _entityDal.GetAsync(i => i.id == request.id);
            
            return new MainResponseDto(_mapper.Map<RoleDto>(entity), permission.permittedActions);
        }
    }
}