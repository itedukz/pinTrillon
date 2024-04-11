using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Products;

namespace ms.MainApi.Business.Cqrs.Products.Brands.Validators;

public class BrandDeleteCommandValidator : AbstractValidator<BrandDeleteCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IBrandDal _entityDal;

    public BrandDeleteCommandValidator(IMessagesRepository messagesRepository,
        IBrandDal entityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;

        RuleFor(v => v.id)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("Id", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistEntity(ctx.id, token))
            .WithMessage(_messagesRepository.NotFound("Brand"));

    }

    private async Task<bool> IsExistEntity(int id, CancellationToken token)
        => await _entityDal.AnyAsync(i => i.id == id);

}