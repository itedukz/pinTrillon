using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess;
using ms.MainApi.DataAccess.Products;

namespace ms.MainApi.Business.Cqrs.Products.ProductArticles.Validators;

public class ProductArticleCreateCommandValidator : AbstractValidator<ProductArticleCreateCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IProductArticleDal _entityDal;
    private readonly ICatalogDal _catalogDal;

    public ProductArticleCreateCommandValidator(IMessagesRepository messagesRepository, 
        IProductArticleDal entityDal, ICatalogDal catalogDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;
        _catalogDal = catalogDal;

        RuleFor(x => x.form.name)
            .NotEmpty()
            .WithMessage(_messagesRepository.NotEmpty("ProductArticle name "))
            .MustAsync(async (ctx, context, token) =>
                await IsUniqueName(ctx.form.name, token))
            .WithMessage(_messagesRepository.ShouldBeUnique("ProductArticle name"));

        RuleFor(v => v.form.catalogId)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("catalogId", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistCatalog(ctx.form.catalogId, token))
            .WithMessage(_messagesRepository.NotFound("Product Catalog"));

    }

    private async Task<bool> IsUniqueName(string name, CancellationToken token)
    {
        var data = await _entityDal.GetAsync(i => i.name.ToLower() == name.ToLower());
        return data == null;
    }

    private async Task<bool> IsExistCatalog(int id, CancellationToken token)
        => await _catalogDal.AnyAsync(i => i.id == id);

}