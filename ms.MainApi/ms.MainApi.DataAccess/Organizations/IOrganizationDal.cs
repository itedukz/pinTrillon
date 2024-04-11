using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels.Organizations;

namespace ms.MainApi.DataAccess.Organizations;

public interface IOrganizationDal : IEntityRepository<Organization> { }

public class OrganizationDal : EntityRepositoryBase<Organization, dbContext>, IOrganizationDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public OrganizationDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}