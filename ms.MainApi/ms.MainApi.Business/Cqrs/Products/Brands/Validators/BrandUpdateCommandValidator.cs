using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Products;

namespace ms.MainApi.Business.Cqrs.Products.Brands.Validators;

public class BrandUpdateCommandValidator : AbstractValidator<BrandUpdateCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IBrandDal _entityDal;

    public BrandUpdateCommandValidator(IMessagesRepository messagesRepository,
        IBrandDal entityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;

        RuleFor(v => v.form.id)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("Id", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistEntity(ctx.form.id, token))
            .WithMessage(_messagesRepository.NotFound("Brand"));

        RuleFor(x => x.form.name)
            .NotEmpty()
            .WithMessage(_messagesRepository.NotEmpty("Brand name "))
            .MustAsync(async (ctx, context, token) =>
                await IsUniqueName(ctx.form.id, ctx.form.name, token))
            .WithMessage(_messagesRepository.ShouldBeUnique("Brand name"));

    }

    private async Task<bool> IsUniqueName(int id, string name, CancellationToken token)
    {
        var data = await _entityDal.GetAsync(i => i.id != id && i.name.ToLower() == name.ToLower());
        return data == null;
    }

    private async Task<bool> IsExistEntity(int id, CancellationToken token)
        => await _entityDal.AnyAsync(i => i.id == id);

}