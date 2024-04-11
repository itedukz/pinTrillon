using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Projects;

namespace ms.MainApi.Business.Cqrs.Projects.Validators;

public class ProjectUpdateCommandValidator : AbstractValidator<ProjectUpdateCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IProjectDal _entityDal;
    private readonly IProjectCatalogDal _catalogDal;

    public ProjectUpdateCommandValidator(IMessagesRepository messagesRepository, IProjectDal entityDal,
        IProjectCatalogDal catalogDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;
        _catalogDal = catalogDal;

        RuleFor(v => v.form.id)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("Project Id", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExist(ctx.form.id, token))
            .WithMessage(_messagesRepository.NotFound("Project"));

        RuleFor(x => x.form.name)
            .NotEmpty()
            .WithMessage(_messagesRepository.NotEmpty("Project name"));

        RuleFor(x => x.form.price)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEmpty("Project price"));

        RuleFor(x => x.form.measure)
            .NotEmpty()
            .WithMessage(_messagesRepository.NotEmpty("Project measure"));

        RuleFor(x => x.form.measure!.height)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEmpty("Project measure height"));

        RuleFor(x => x.form.measure!.width)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEmpty("Project measure width"));

        RuleFor(x => x.form.measure!.length)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEmpty("Project measure length"));

        RuleFor(v => v.form.projectCatalogId)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("catalogId", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistCatalog(ctx.form.projectCatalogId, token))
            .WithMessage(_messagesRepository.NotFound("Project Catalog"));

    }

    private async Task<bool> IsExist(int id, CancellationToken token)
        => await _entityDal.AnyAsync(i => i.id == id);

    private async Task<bool> IsExistCatalog(int id, CancellationToken token)
        => await _catalogDal.AnyAsync(i => i.id == id);

}