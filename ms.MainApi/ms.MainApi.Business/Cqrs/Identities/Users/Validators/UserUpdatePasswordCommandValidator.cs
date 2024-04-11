using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;

namespace ms.MainApi.Business.Cqrs.Identities.Users.Validators;

public class UserUpdatePasswordCommandValidator : AbstractValidator<UserUpdatePasswordCommand>
{
    private readonly IMessagesRepository _messagesRepository;

    public UserUpdatePasswordCommandValidator(IMessagesRepository messagesRepository)
    {
        _messagesRepository = messagesRepository;

        RuleFor(x => x.form.password)
            .NotEmpty()
            .WithMessage(_messagesRepository.NotEmpty("Password"))
            .MinimumLength(4)
            .WithMessage(_messagesRepository.WrongDataFormat("Minimum Password length 4"));

    }
}