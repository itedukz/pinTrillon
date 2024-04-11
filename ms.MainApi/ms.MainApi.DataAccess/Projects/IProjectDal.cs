using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels.Projects;

namespace ms.MainApi.DataAccess.Projects;

public interface IProjectDal : IEntityRepository<Project> { }

public class ProjectDal : EntityRepositoryBase<Project, dbContext>, IProjectDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public ProjectDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}