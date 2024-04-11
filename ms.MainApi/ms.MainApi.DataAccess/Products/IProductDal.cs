using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels.Products;

namespace ms.MainApi.DataAccess.Products;

public interface IProductDal : IEntityRepository<Product> { }


public class ProductDal : EntityRepositoryBase<Product, dbContext>, IProductDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public ProductDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}