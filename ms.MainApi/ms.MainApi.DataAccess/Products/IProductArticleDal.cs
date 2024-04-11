using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels.Products;

namespace ms.MainApi.DataAccess.Products;

public interface IProductArticleDal : IEntityRepository<ProductArticle> { }

public class ProductArticleDal : EntityRepositoryBase<ProductArticle, dbContext>, IProductArticleDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public ProductArticleDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}