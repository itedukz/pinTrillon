using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.Entity.Models.Enums;
using System;

namespace ms.MainApi.Business.Cqrs.Identities.Permissions.Validators;

public class PermissionCreateCommandValidator : AbstractValidator<PermissionCreateCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IRoleDal _roleDal;
    private readonly IPermissionDal _permissionDal;

    public PermissionCreateCommandValidator(IMessagesRepository messagesRepository, IRoleDal roleDal, IPermissionDal permissionDal)
    {
        _messagesRepository = messagesRepository;
        _roleDal = roleDal;
        _permissionDal = permissionDal;

        RuleFor(v => v.form.roleId)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("roleId", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistRole(ctx.form.roleId, token))
            .WithMessage(_messagesRepository.NotFound("Role"));

        RuleFor(v => v.form.permissionId)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("permissionId", "0"))
            .Must(IsExistPermission)
            .WithMessage(_messagesRepository.NotFound("Permission"));

        RuleFor(v => v.form.actions)
            .Must(IsExistActions)
            .WithMessage(_messagesRepository.NotFound("Actions"));
    }

    private async Task<bool> IsExistRole(int roleId, CancellationToken token)
        => await _roleDal.AnyAsync(i => i.id == roleId);


    private async Task<bool> IsExistPermissionDal(int permissionId, CancellationToken token)
        => await _permissionDal.AnyAsync(i => i.id == permissionId);
    
    private bool IsExistPermission(int permissionId)
        => Enum.IsDefined(typeof(PermissionName), permissionId);


    private bool IsExistActions(List<int>? actions)
    {
        if (actions != null && actions.Count > 0)
        {
            if (actions.Count == 1 && actions[0] == 0)
                return false;

            bool isEnumNotFound = true;
            foreach (int actionId in actions)
                if (!Enum.IsDefined(typeof(PermissionAction), actionId))
                {
                    isEnumNotFound = false;
                    break;
                }

            return isEnumNotFound;
        }

        return false;
    }
}