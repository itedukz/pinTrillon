using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels.Projects;

namespace ms.MainApi.DataAccess.Projects;

public interface IProjectLayoutDal : IEntityRepository<ProjectLayout> { }

public class ProjectLayoutDal : EntityRepositoryBase<ProjectLayout, dbContext>, IProjectLayoutDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public ProjectLayoutDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}