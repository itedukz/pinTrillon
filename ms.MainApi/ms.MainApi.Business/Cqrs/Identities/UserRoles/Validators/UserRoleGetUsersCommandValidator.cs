using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;

namespace ms.MainApi.Business.Cqrs.Identities.UserRoles.Validators;

public class UserRoleGetUsersCommandValidator : AbstractValidator<UserRoleGetUsersCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IRoleDal _roleDal;

    public UserRoleGetUsersCommandValidator(IMessagesRepository messagesRepository, IRoleDal roleDal)
    {
        _messagesRepository = messagesRepository;
        _roleDal = roleDal;

        RuleFor(v => v.roleId)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("userId", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistRole(ctx.roleId, token))
            .WithMessage(_messagesRepository.NotFound("User"));

    }

    private async Task<bool> IsExistRole(int roleId, CancellationToken token)
        => await _roleDal.AnyAsync(i => i.id == roleId);


}