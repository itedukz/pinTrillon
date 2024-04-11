using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Projects;

namespace ms.MainApi.Business.Cqrs.Projects.Validators;

public class ProjectDeleteCommandValidator : AbstractValidator<ProjectDeleteCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IProjectDal _entityDal;

    public ProjectDeleteCommandValidator(IMessagesRepository messagesRepository, IProjectDal entityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;

        RuleFor(v => v.id)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("Project Id", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExist(ctx.id, token))
            .WithMessage(_messagesRepository.NotFound("Project"));

    }

    private async Task<bool> IsExist(int id, CancellationToken token)
        => await _entityDal.AnyAsync(i => i.id == id);
}