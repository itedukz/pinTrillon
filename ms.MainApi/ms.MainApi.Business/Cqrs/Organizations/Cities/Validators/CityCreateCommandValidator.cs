using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Organizations;

namespace ms.MainApi.Business.Cqrs.Organizations.Cities.Validators;

public class CityCreateCommandValidator : AbstractValidator<CityCreateCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly ICityDal _entityDal;

    public CityCreateCommandValidator(IMessagesRepository messagesRepository,
        ICityDal entityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;

        RuleFor(x => x.form.name)
            .NotEmpty()
            .WithMessage(_messagesRepository.NotEmpty("City name "))
            .MustAsync(async (ctx, context, token) =>
                await IsUniqueName(ctx.form.name, token))
            .WithMessage(_messagesRepository.ShouldBeUnique("City name"));

    }

    private async Task<bool> IsUniqueName(string name, CancellationToken token)
    {
        var data = await _entityDal.GetAsync(i => i.name.ToLower() == name.ToLower());
        return data == null;
    }
}