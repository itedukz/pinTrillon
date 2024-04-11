using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Products;

namespace ms.MainApi.Business.Cqrs.Baskets.Validators;

public class BasketAddProductCommandValidator : AbstractValidator<BasketAddProductCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IProductDal _entityDal;

    public BasketAddProductCommandValidator(IMessagesRepository messagesRepository, IProductDal entityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;

        RuleFor(v => v.productId)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("ProductId", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExist(ctx.productId, token))
            .WithMessage(_messagesRepository.NotFound("Product"));

    }

    private async Task<bool> IsExist(int id, CancellationToken token)
        => await _entityDal.AnyAsync(i => i.id == id);
}