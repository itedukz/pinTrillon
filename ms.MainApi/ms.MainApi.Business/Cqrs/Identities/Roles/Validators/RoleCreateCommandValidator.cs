using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;

namespace ms.MainApi.Business.Cqrs.Identities.Roles.Validators;

public class RoleCreateCommandValidator : AbstractValidator<RoleCreateCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IRoleDal _entityDal;

    public RoleCreateCommandValidator(IMessagesRepository messagesRepository,
        IRoleDal entityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;

        RuleFor(x => x.form.name)
            .NotEmpty()
            .WithMessage(_messagesRepository.NotEmpty("Role name "))
            .MustAsync(async (ctx, context, token) =>
                await IsUniqueName(ctx.form.name, token))
            .WithMessage(_messagesRepository.ShouldBeUnique("Role name"));

    }

    private async Task<bool> IsUniqueName(string name, CancellationToken token)
    {
        var data = await _entityDal.GetAsync(i => i.name.ToLower() == name.ToLower());
        return data == null;
    }
}