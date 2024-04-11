using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels.Organizations;

namespace ms.MainApi.DataAccess.Organizations;

public interface IOrganizationPictureDal : IEntityRepository<OrganizationPicture> { }

public class OrganizationPictureDal : EntityRepositoryBase<OrganizationPicture, dbContext>, IOrganizationPictureDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public OrganizationPictureDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}