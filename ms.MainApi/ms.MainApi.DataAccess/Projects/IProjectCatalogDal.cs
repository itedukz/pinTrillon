using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels.Projects;

namespace ms.MainApi.DataAccess.Projects;

public interface IProjectCatalogDal : IEntityRepository<ProjectCatalog> { }

public class ProjectCatalogDal : EntityRepositoryBase<ProjectCatalog, dbContext>, IProjectCatalogDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public ProjectCatalogDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}