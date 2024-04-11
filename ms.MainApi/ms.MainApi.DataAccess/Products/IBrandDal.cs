using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels.Products;

namespace ms.MainApi.DataAccess.Products;

public interface IBrandDal : IEntityRepository<Brand> { }

public class BrandDal : EntityRepositoryBase<Brand, dbContext>, IBrandDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public BrandDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}