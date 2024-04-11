using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess;

namespace ms.MainApi.Business.Cqrs.Catalogs.Validators;

public class CatalogDeleteCommandValidator : AbstractValidator<CatalogDeleteCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly ICatalogDal _entityDal;

    public CatalogDeleteCommandValidator(IMessagesRepository messagesRepository,
        ICatalogDal entityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;

        RuleFor(v => v.id)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("Id", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistEntity(ctx.id, token))
            .WithMessage(_messagesRepository.NotFound("Catalog"));

    }

    private async Task<bool> IsExistEntity(int id, CancellationToken token)
        => await _entityDal.AnyAsync(i => i.id == id);

}