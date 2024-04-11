using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;

namespace ms.MainApi.Business.Cqrs.Identities.Users.Validators;

public class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IUserDal _userDal;

    public UserCreateCommandValidator(IMessagesRepository messagesRepository, IUserDal userDal)
    {
        _messagesRepository = messagesRepository;
        _userDal = userDal;

        RuleFor(x => x.form.firstName)
            .NotEmpty()
            .WithMessage(_messagesRepository.NotEmpty("User name"));

        RuleFor(x => x.form.email)
            .NotEmpty()
            .WithMessage(_messagesRepository.NotEmpty("User login "))
            .EmailAddress()
            .WithMessage(_messagesRepository.WrongDataFormat("Email "))
            .MustAsync(async (ctx, context, token) =>
                await IsUniqueLogin(ctx.form.email, token))
            .WithMessage(_messagesRepository.ShouldBeUnique("User Login"));

        RuleFor(x => x.form.password)
            .NotEmpty()
            .WithMessage(_messagesRepository.NotEmpty("Password"))
            .MinimumLength(3)
            .WithMessage(_messagesRepository.WrongDataFormat("Password length"));

        //RuleFor(x => x.form.phoneNumber)
        //    .NotEmpty()
        //    .WithMessage(_messagesRepository.NotEmpty("phoneNumber"));
    }

    private async Task<bool> IsUniqueLogin(string login, CancellationToken token)
    {
        var data = await _userDal.GetAsync(i => i.email.ToLower() == login.ToLower());
        return data == null;
    }
}