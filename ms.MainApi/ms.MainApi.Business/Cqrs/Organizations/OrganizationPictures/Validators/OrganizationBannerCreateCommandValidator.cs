using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Organizations;

namespace ms.MainApi.Business.Cqrs.Organizations.OrganizationPictures.Validators;

public class OrganizationBannerCreateCommandValidator : AbstractValidator<OrganizationBannerCreateCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IOrganizationPictureDal _entityDal;

    public OrganizationBannerCreateCommandValidator(IMessagesRepository messagesRepository,
        IOrganizationPictureDal entityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;

        RuleFor(x => x.organizationId)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("organizationId", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsOrganizationBanner(ctx.organizationId, token))
            .WithMessage(_messagesRepository.ShouldBeUnique("Organization banner already have"));

    }

    private async Task<bool> IsOrganizationBanner(int organizationId, CancellationToken token)
    {
        var data = await _entityDal.GetAsync(i => i.organizationId == organizationId);
        return data == null;
    }
}