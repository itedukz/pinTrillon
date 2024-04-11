using AutoMapper;
using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.Entity.Models.DbModels.Identity;
using ms.MainApi.Entity.Models.Dtos;
using ms.MainApi.Entity.Models.Dtos.Identities.Permissions;
using ms.MainApi.Entity.Models.Dtos.Identities.Roles;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Identities.Permissions;

public class PermissionCreateCommand : IRequest<IMainResponseDto>
{
    public PermissionCreateDto form { get; }

    public PermissionCreateCommand(PermissionCreateDto _form)
    {
        form = _form;
    }

    public class Handler : IRequestHandler<PermissionCreateCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IPermissionDal _entityDal;
        private readonly IRoleDal _roleDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<PermissionCreateCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IMapper mapper, IPermissionDal entityDal, IRoleDal roleDal, IPermissionCheck checkPermission,
            IValidator<PermissionCreateCommand> validator, IMessagesRepository messagesRepository)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _roleDal = roleDal;
            _checkPermission = checkPermission;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(PermissionCreateCommand request, CancellationToken cancellationToken)
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
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.permission, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion


            Permission? permissionForDelete = await _entityDal.GetAsync(i => i.roleId == request.form.roleId && i.permissionId == request.form.permissionId);
            if (permissionForDelete != null)
                await _entityDal.DeleteAsync(permissionForDelete);

            Permission entity = new Permission
            {
                roleId = request.form.roleId,
                permissionId = request.form.permissionId,
                actions = request.form.actions
            };
            await _entityDal.AddAsync(entity);

            PermissionDto entityDto = new PermissionDto
            {
                permission = PermissionConvert.toBaseClass(entity.permissionId),
                actions = createdActions(entity.actions),
                role = _mapper.Map<RoleDto>(await _roleDal.GetAsync(i => i.id == entity.roleId))
            };

            return new MainResponseDto(entityDto, permission.permittedActions);
        }

        private List<EnumItemDto> createdActions(List<int>? allowedActions)
        {
            List<EnumItemDto> list = new List<EnumItemDto>();

            if (allowedActions != null && allowedActions.Count > 0)
                foreach (int item in allowedActions)
                {
                    switch (item)
                    {
                        case (int)PermissionAction.getAll: 
                            list.Add(new EnumItemDto { id = (int)PermissionAction.getAll, name = PermissionAction.getAll.ToString() }); break;
                        case (int)PermissionAction.getOwn:
                            list.Add(new EnumItemDto { id = (int)PermissionAction.getOwn, name = PermissionAction.getOwn.ToString() }); break;
                        case (int)PermissionAction.create:
                            list.Add(new EnumItemDto { id = (int)PermissionAction.create, name = PermissionAction.create.ToString() }); break;
                        case (int)PermissionAction.update:
                            list.Add(new EnumItemDto { id = (int)PermissionAction.update, name = PermissionAction.update.ToString() }); break;
                        case (int)PermissionAction.delete:
                            list.Add(new EnumItemDto { id = (int)PermissionAction.delete, name = PermissionAction.delete.ToString() }); break;
                    }
                }

            return list;
        }
    }
}