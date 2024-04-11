using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess;
using ms.MainApi.DataAccess.Products;

namespace ms.MainApi.Business.Cqrs.Products.Validators;

public class ProductUpdateCommandValidator : AbstractValidator<ProductUpdateCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IProductDal _entityDal;
    private readonly IProductArticleDal _productArticleDal;
    private readonly IBrandDal _brandDal;
    private readonly IMaterialDal _materialDal;
    private readonly ICatalogDal _catalogDal;

    public ProductUpdateCommandValidator(IMessagesRepository messagesRepository, IProductDal entityDal, 
        IProductArticleDal productArticleDal, IBrandDal brandDal, IMaterialDal materialDal, ICatalogDal catalogDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;
        _productArticleDal = productArticleDal;
        _brandDal = brandDal;
        _materialDal = materialDal;
        _catalogDal = catalogDal;

        RuleFor(v => v.form.id)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("Product Id", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExist(ctx.form.id, token))
            .WithMessage(_messagesRepository.NotFound("Product"));

        RuleFor(x => x.form.name)
            .NotEmpty()
            .WithMessage(_messagesRepository.NotEmpty("Product name"));

        RuleFor(x => x.form.price)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEmpty("Product price"));

        RuleFor(x => x.form.measure)
            .NotEmpty()
            .WithMessage(_messagesRepository.NotEmpty("Product measure"));

        RuleFor(x => x.form.measure!.height)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEmpty("Product measure height"));

        RuleFor(x => x.form.measure!.width)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEmpty("Product measure width"));

        RuleFor(x => x.form.measure!.length)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEmpty("Product measure length"));

        RuleFor(v => v.form.productArticleId)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("productArticleId", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistArticle(ctx.form.productArticleId, token))
            .WithMessage(_messagesRepository.NotFound("ProductArticle"));

        RuleFor(v => v.form.catalogId)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("catalogId", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistCatalog(ctx.form.catalogId, token))
            .WithMessage(_messagesRepository.NotFound("Product Catalog"));

        RuleFor(v => v.form.brandId)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("brandId", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistBrand(ctx.form.brandId, token))
            .WithMessage(_messagesRepository.NotFound("Brand"));

        RuleFor(v => v.form.materialsId)
            .MustAsync(async (ctx, context, token) =>
                await IsExistMaterials(ctx.form.materialsId, token))
            .WithMessage(_messagesRepository.NotFound("Materials"));

    }

    private async Task<bool> IsExist(int id, CancellationToken token)
        => await _entityDal.AnyAsync(i => i.id == id);

    private async Task<bool> IsExistArticle(int id, CancellationToken token)
        => await _productArticleDal.AnyAsync(i => i.id == id);

    private async Task<bool> IsExistCatalog(int id, CancellationToken token)
        => await _catalogDal.AnyAsync(i => i.id == id);

    private async Task<bool> IsExistBrand(int id, CancellationToken token)
        => await _brandDal.AnyAsync(i => i.id == id);

    private async Task<bool> IsExistMaterials(List<int>? materialsId, CancellationToken token)
    {
        if (materialsId != null && materialsId.Count > 0)
        {
            bool isMaterialNotFound = true;
            foreach (int materialId in materialsId)
            {
                if (!await _materialDal.AnyAsync(i => i.id == materialId))
                {
                    isMaterialNotFound = false;
                    break;
                }
            }
            return isMaterialNotFound;
        }

        return true;
    }
}