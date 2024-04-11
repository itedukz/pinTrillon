using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Organizations;

namespace ms.MainApi.Business.Cqrs.Organizations.OrganizationPictures.Validators;

public class OrganizationAvatarCreateCommandValidator : AbstractValidator<OrganizationAvatarCreateCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IOrganizationDal _entityDal;

    public OrganizationAvatarCreateCommandValidator(IMessagesRepository messagesRepository, IOrganizationDal entityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;

        RuleFor(x => x.organizationId)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("organizationId", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsEntityExist(ctx.organizationId, token))
            .WithMessage(_messagesRepository.NotFound("Organization"));

    }

    private async Task<bool> IsEntityExist(int organizationId, CancellationToken token)
        => await _entityDal.AnyAsync(i => i.id == organizationId);
}