using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess;
using ms.MainApi.DataAccess.Products;

namespace ms.MainApi.Business.Cqrs.Products.ProductArticles.Validators;

public class ProductArticleUpdateCommandValidator : AbstractValidator<ProductArticleUpdateCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IProductArticleDal _entityDal;
    private readonly ICatalogDal _catalogDal;

    public ProductArticleUpdateCommandValidator(IMessagesRepository messagesRepository, 
        IProductArticleDal entityDal, ICatalogDal catalogDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;
        _catalogDal = catalogDal;

        RuleFor(v => v.form.id)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("id", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExist(ctx.form.id, token))
            .WithMessage(_messagesRepository.NotFound("ProductArticle"));

        RuleFor(x => x.form.name)
            .NotEmpty()
            .WithMessage(_messagesRepository.NotEmpty("ProductArticle name"))
            .MustAsync(async (ctx, context, token) =>
                await IsUniqueName(ctx.form.name, ctx.form.id, token))
            .WithMessage(_messagesRepository.ShouldBeUnique("ProductArticle name"));

        RuleFor(v => v.form.catalogId)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("catalogId", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistCatalog(ctx.form.catalogId, token))
            .WithMessage(_messagesRepository.NotFound("Product Catalog"));

    }

    private async Task<bool> IsExist(int id, CancellationToken token)
        => await _entityDal.AnyAsync(i => i.id == id);

    private async Task<bool> IsUniqueName(string name, int Id, CancellationToken token)
    {
        var data = await _entityDal.GetAsync(i => i.name.ToLower() == name.ToLower() && i.id != Id);
        return data == null;
    }

    private async Task<bool> IsExistCatalog(int id, CancellationToken token)
        => await _catalogDal.AnyAsync(i => i.id == id);

}