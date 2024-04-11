using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;

namespace ms.MainApi.Business.Cqrs.Identities.Permissions.Validators;

public class PermissionDeleteCommandValidator : AbstractValidator<PermissionDeleteCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IRoleDal _roleDal;
    private readonly IPermissionDal _permissionDal;

    public PermissionDeleteCommandValidator(IMessagesRepository messagesRepository, IRoleDal roleDal, IPermissionDal permissionDal)
    {
        _messagesRepository = messagesRepository;
        _roleDal = roleDal;
        _permissionDal = permissionDal;

        RuleFor(v => v.roleId)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("roleId", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistRole(ctx.roleId, token))
            .WithMessage(_messagesRepository.NotFound("Role"));

        RuleFor(v => v.permissionId)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("permissionId", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistPermissionDal(ctx.permissionId, token))
            .WithMessage(_messagesRepository.NotFound("Permission"));

    }

    private async Task<bool> IsExistRole(int roleId, CancellationToken token)
        => await _roleDal.AnyAsync(i => i.id == roleId);


    private async Task<bool> IsExistPermissionDal(int permissionId, CancellationToken token)
        => await _permissionDal.AnyAsync(i => i.id == permissionId);

}