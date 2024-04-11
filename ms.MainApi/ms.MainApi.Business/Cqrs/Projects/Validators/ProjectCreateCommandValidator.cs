using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Projects;

namespace ms.MainApi.Business.Cqrs.Projects.Validators;

public class ProjectCreateCommandValidator : AbstractValidator<ProjectCreateCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IProjectCatalogDal _catalogDal;

    public ProjectCreateCommandValidator(IMessagesRepository messagesRepository, IProjectCatalogDal catalogDal)
    {
        _messagesRepository = messagesRepository;        
        _catalogDal = catalogDal;

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

    private async Task<bool> IsExistCatalog(int id, CancellationToken token)
        => await _catalogDal.AnyAsync(i => i.id == id);
    
}