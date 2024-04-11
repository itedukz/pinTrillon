using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;

namespace ms.MainApi.Business.Cqrs.Identities.Users.Validators;

public class UserUpdateCommandValidator : AbstractValidator<UserUpdateCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IUserDal _entityDal;

    public UserUpdateCommandValidator(IMessagesRepository messagesRepository, IUserDal entityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;


        RuleFor(v => v.form.id)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("userId", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistEntity(ctx.form.id, token))
            .WithMessage(_messagesRepository.NotFound("User"));

        RuleFor(x => x.form.firstName)
            .NotEmpty()
            .WithMessage(_messagesRepository.NotEmpty("User name"));

    }

    private async Task<bool> IsExistEntity(int userId, CancellationToken token)
        => await _entityDal.AnyAsync(i => i.id == userId);

}