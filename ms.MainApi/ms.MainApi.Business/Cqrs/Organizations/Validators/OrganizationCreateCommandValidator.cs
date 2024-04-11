using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Organizations;

namespace ms.MainApi.Business.Cqrs.Organizations.Validators;

public class OrganizationCreateCommandValidator : AbstractValidator<OrganizationCreateCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IOrganizationDal _entityDal;
    private readonly ICityDal _cityDal;

    public OrganizationCreateCommandValidator(IMessagesRepository messagesRepository,
        IOrganizationDal entityDal, ICityDal cityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;
        _cityDal = cityDal;

        RuleFor(x => x.form.name)
            .NotEmpty()
            .WithMessage(_messagesRepository.NotEmpty("Organization name "))
            .MustAsync(async (ctx, context, token) =>
                await IsUniqueName(ctx.form.name, token))
            .WithMessage(_messagesRepository.ShouldBeUnique("Organization name"));

        RuleFor(v => v.form.cityId)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("cityId", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistCity(ctx.form.cityId, token))
            .WithMessage(_messagesRepository.NotFound("City"));

    }

    private async Task<bool> IsUniqueName(string name, CancellationToken token)
    {
        var data = await _entityDal.GetAsync(i => i.name.ToLower() == name.ToLower());
        return data == null;
    }

    private async Task<bool> IsExistCity(int id, CancellationToken token)
        => await _cityDal.AnyAsync(i => i.id == id);

}