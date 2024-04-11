using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Organizations;

namespace ms.MainApi.Business.Cqrs.Organizations.Validators;

public class OrganizationGetCommandValidator : AbstractValidator<OrganizationGetCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IOrganizationDal _entityDal;

    public OrganizationGetCommandValidator(IMessagesRepository messagesRepository,
        IOrganizationDal entityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;


        RuleFor(v => v.id)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("id", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExist(ctx.id, token))
            .WithMessage(_messagesRepository.NotFound("Organization"));

    }

    private async Task<bool> IsExist(int id, CancellationToken token)
        => await _entityDal.AnyAsync(i => i.id == id);

}