using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels.Organizations;

namespace ms.MainApi.DataAccess.Organizations;

public interface ICityDal : IEntityRepository<City> { }

public class CityDal : EntityRepositoryBase<City, dbContext>, ICityDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public CityDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}