using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Products;

namespace ms.MainApi.Business.Cqrs.Products.Materials.Validators;

public class MaterialGetCommandValidator : AbstractValidator<MaterialGetCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IMaterialDal _entityDal;

    public MaterialGetCommandValidator(IMessagesRepository messagesRepository,
        IMaterialDal entityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;

        RuleFor(v => v.id)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("Id", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistEntity(ctx.id, token))
            .WithMessage(_messagesRepository.NotFound("Material"));

    }

    private async Task<bool> IsExistEntity(int id, CancellationToken token)
        => await _entityDal.AnyAsync(i => i.id == id);

}