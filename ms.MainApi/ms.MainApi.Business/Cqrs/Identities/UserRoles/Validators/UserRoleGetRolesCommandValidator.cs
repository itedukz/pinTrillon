using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;

namespace ms.MainApi.Business.Cqrs.Identities.UserRoles.Validators;

public class UserRoleGetRolesCommandValidator : AbstractValidator<UserRoleGetRolesCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IUserDal _userDal;

    public UserRoleGetRolesCommandValidator(IMessagesRepository messagesRepository, IUserDal userDal)
    {
        _messagesRepository = messagesRepository;
        _userDal = userDal;

        RuleFor(v => v.userId)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("userId", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistUser(ctx.userId, token))
            .WithMessage(_messagesRepository.NotFound("User"));

    }

    private async Task<bool> IsExistUser(int userId, CancellationToken token)
        => await _userDal.AnyAsync(i => i.id == userId);

}