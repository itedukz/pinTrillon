using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess;

namespace ms.MainApi.Business.Cqrs.Catalogs.Validators;

public class CatalogCreateCommandValidator : AbstractValidator<CatalogCreateCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly ICatalogDal _entityDal;

    public CatalogCreateCommandValidator(IMessagesRepository messagesRepository,
        ICatalogDal entityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;

        RuleFor(x => x.form.name)
            .NotEmpty()
            .WithMessage(_messagesRepository.NotEmpty("Catalog name "))
            .MustAsync(async (ctx, context, token) =>
                await IsUniqueName(ctx.form.name, token))
            .WithMessage(_messagesRepository.ShouldBeUnique("Catalog name"));

    }

    private async Task<bool> IsUniqueName(string name, CancellationToken token)
    {
        var data = await _entityDal.GetAsync(i => i.name.ToLower() == name.ToLower());
        return data == null;
    }
}