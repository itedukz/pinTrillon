using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;

namespace ms.MainApi.Business.Cqrs.Identities.Users.Validators;

public class UserGetCommandValidator : AbstractValidator<UserGetCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IUserDal _entityDal;

    public UserGetCommandValidator(IMessagesRepository messagesRepository, IUserDal entityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;

        RuleFor(v => v.id)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("userId", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistEntity(ctx.id, token))
            .WithMessage(_messagesRepository.NotFound("User"));

    }

    private async Task<bool> IsExistEntity(int userId, CancellationToken token)
        => await _entityDal.AnyAsync(i => i.id == userId);
}