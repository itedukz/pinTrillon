using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Projects;

namespace ms.MainApi.Business.Cqrs.Favourites.Validators;

public class FavouriteAddProjectCommandValidator : AbstractValidator<FavouriteAddProjectCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IProjectDal _entityDal;

    public FavouriteAddProjectCommandValidator(IMessagesRepository messagesRepository, IProjectDal entityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;

        RuleFor(v => v.projectId)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("ProjectId", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExist(ctx.projectId, token))
            .WithMessage(_messagesRepository.NotFound("Project"));

    }

    private async Task<bool> IsExist(int id, CancellationToken token)
        => await _entityDal.AnyAsync(i => i.id == id);
}