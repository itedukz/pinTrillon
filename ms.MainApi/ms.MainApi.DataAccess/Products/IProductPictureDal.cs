using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels.Products;

namespace ms.MainApi.DataAccess.Products;

public interface IProductPictureDal : IEntityRepository<ProductPicture> { }


public class ProductPictureDal : EntityRepositoryBase<ProductPicture, dbContext>, IProductPictureDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public ProductPictureDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}