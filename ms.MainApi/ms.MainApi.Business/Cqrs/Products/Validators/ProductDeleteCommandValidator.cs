using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Products;

namespace ms.MainApi.Business.Cqrs.Products.Validators;

public class ProductDeleteCommandValidator : AbstractValidator<ProductDeleteCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IProductDal _entityDal;

    public ProductDeleteCommandValidator(IMessagesRepository messagesRepository, IProductDal entityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;

        RuleFor(v => v.id)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("Product Id", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExist(ctx.id, token))
            .WithMessage(_messagesRepository.NotFound("Product"));

    }

    private async Task<bool> IsExist(int id, CancellationToken token)
        => await _entityDal.AnyAsync(i => i.id == id);
}