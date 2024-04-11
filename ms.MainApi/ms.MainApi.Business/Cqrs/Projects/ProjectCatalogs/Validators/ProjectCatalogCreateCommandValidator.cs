using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Projects;

namespace ms.MainApi.Business.Cqrs.Projects.ProjectCatalogs.Validators;

public class ProjectCatalogCreateCommandValidator : AbstractValidator<ProjectCatalogCreateCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IProjectCatalogDal _entityDal;

    public ProjectCatalogCreateCommandValidator(IMessagesRepository messagesRepository,
        IProjectCatalogDal entityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;

        RuleFor(x => x.form.name)
            .NotEmpty()
            .WithMessage(_messagesRepository.NotEmpty("ProjectCatalog name "))
            .MustAsync(async (ctx, context, token) =>
                await IsUniqueName(ctx.form.name, token))
            .WithMessage(_messagesRepository.ShouldBeUnique("ProjectCatalog name"));

    }

    private async Task<bool> IsUniqueName(string name, CancellationToken token)
    {
        var data = await _entityDal.GetAsync(i => i.name.ToLower() == name.ToLower());
        return data == null;
    }
}