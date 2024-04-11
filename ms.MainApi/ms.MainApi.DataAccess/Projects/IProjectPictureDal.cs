using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels.Projects;

namespace ms.MainApi.DataAccess.Projects;

public interface IProjectPictureDal : IEntityRepository<ProjectPicture> { }

public class ProjectPictureDal : EntityRepositoryBase<ProjectPicture, dbContext>, IProjectPictureDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public ProjectPictureDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}