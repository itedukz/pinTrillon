using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Organizations;

namespace ms.MainApi.Business.Cqrs.Organizations.Validators;

public class OrganizationUpdateCommandValidator : AbstractValidator<OrganizationUpdateCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IOrganizationDal _entityDal;
    private readonly ICityDal _cityDal;

    public OrganizationUpdateCommandValidator(IMessagesRepository messagesRepository,
        IOrganizationDal entityDal, ICityDal cityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;
        _cityDal = cityDal;


        RuleFor(v => v.form.id)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("id", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExist(ctx.form.id, token))
            .WithMessage(_messagesRepository.NotFound("Organization"));

        RuleFor(x => x.form.name)
            .NotEmpty()
            .WithMessage(_messagesRepository.NotEmpty("Organization name "))
            .MustAsync(async (ctx, context, token) =>
                await IsUniqueName(ctx.form.name, ctx.form.id, token))
            .WithMessage(_messagesRepository.ShouldBeUnique("Organization name"));

        RuleFor(v => v.form.cityId)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("cityId", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistCity(ctx.form.cityId, token))
            .WithMessage(_messagesRepository.NotFound("City"));

    }


    private async Task<bool> IsExist(int id, CancellationToken token)
        => await _entityDal.AnyAsync(i => i.id == id);

    private async Task<bool> IsUniqueName(string name, int Id, CancellationToken token)
    {
        var data = await _entityDal.GetAsync(i => i.name.ToLower() == name.ToLower() && i.id != Id);
        return data == null;
    }

    private async Task<bool> IsExistCity(int id, CancellationToken token)
        => await _cityDal.AnyAsync(i => i.id == id);

}