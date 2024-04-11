using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Organizations;

namespace ms.MainApi.Business.Cqrs.Organizations.Cities.Validators;

public class CityGetCommandValidator : AbstractValidator<CityGetCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly ICityDal _entityDal;

    public CityGetCommandValidator(IMessagesRepository messagesRepository,
        ICityDal entityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;

        RuleFor(v => v.id)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("Id", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistEntity(ctx.id, token))
            .WithMessage(_messagesRepository.NotFound("City"));

    }

    private async Task<bool> IsExistEntity(int id, CancellationToken token)
        => await _entityDal.AnyAsync(i => i.id == id);

}