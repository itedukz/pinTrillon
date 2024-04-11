﻿using FluentValidation;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Projects;

namespace ms.MainApi.Business.Cqrs.Projects.ProjectCatalogs.Validators;

public class ProjectCatalogGetCommandValidator : AbstractValidator<ProjectCatalogGetCommand>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IProjectCatalogDal _entityDal;

    public ProjectCatalogGetCommandValidator(IMessagesRepository messagesRepository,
        IProjectCatalogDal entityDal)
    {
        _messagesRepository = messagesRepository;
        _entityDal = entityDal;

        RuleFor(v => v.id)
            .GreaterThan(0)
            .WithMessage(_messagesRepository.NotEqual("Id", "0"))
            .MustAsync(async (ctx, context, token) =>
                await IsExistEntity(ctx.id, token))
            .WithMessage(_messagesRepository.NotFound("ProjectCatalog"));

    }

    private async Task<bool> IsExistEntity(int id, CancellationToken token)
        => await _entityDal.AnyAsync(i => i.id == id);

}