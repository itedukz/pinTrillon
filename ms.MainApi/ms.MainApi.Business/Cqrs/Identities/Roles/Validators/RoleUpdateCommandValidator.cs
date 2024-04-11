using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;

namespace ms.MainApi.Business.Cqrs.Identities.Roles.Validators;

public class RoleUpdateCommandValidator : AbstractValidator<RoleUpdateCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IRoleDal _entityDal;

    public RoleUpdateCommandValidator(IMessagesRepository messagesRepository,
        IRoleDal entityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;

        RuleFor(v => v.form.id)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("Id", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistEntity(ctx.form.id, token))
            .WithMessage(_messagesRepository.NotFound("Role"));

        RuleFor(x => x.form.name)
            .NotEmpty()
            .WithMessage(_messagesRepository.NotEmpty("Role name "))
            .MustAsync(async (ctx, context, token) =>
                await IsUniqueName(ctx.form.id, ctx.form.name, token))
            .WithMessage(_messagesRepository.ShouldBeUnique("Role name"));

    }

    private async Task<bool> IsUniqueName(int id, string name, CancellationToken token)
    {
        var data = await _entityDal.GetAsync(i => i.id != id && i.name.ToLower() == name.ToLower());
        return data == null;
    }

    private async Task<bool> IsExistEntity(int id, CancellationToken token)
        => await _entityDal.AnyAsync(i => i.id == id);

}