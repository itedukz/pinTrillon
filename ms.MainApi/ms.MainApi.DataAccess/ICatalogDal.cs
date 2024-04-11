using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels.Catalogs;

namespace ms.MainApi.DataAccess;

public interface ICatalogDal : IEntityRepository<Catalog> { }

public class CatalogDal : EntityRepositoryBase<Catalog, dbContext>, ICatalogDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public CatalogDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}