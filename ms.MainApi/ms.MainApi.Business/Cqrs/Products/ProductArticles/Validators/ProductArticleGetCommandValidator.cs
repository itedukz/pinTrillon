using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Products;

namespace ms.MainApi.Business.Cqrs.Products.ProductArticles.Validators;

public class ProductArticleGetCommandValidator : AbstractValidator<ProductArticleGetCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IProductArticleDal _entityDal;

    public ProductArticleGetCommandValidator(IMessagesRepository messagesRepository, IProductArticleDal entityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;

        RuleFor(v => v.id)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("id", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExist(ctx.id, token))
            .WithMessage(_messagesRepository.NotFound("ProductArticle"));
    }

    private async Task<bool> IsExist(int id, CancellationToken token)
        => await _entityDal.AnyAsync(i => i.id == id);
}