using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels.Baskets;

namespace ms.MainApi.DataAccess.Baskets;

public interface IBasketProductDal : IEntityRepository<BasketProduct> { }

public class BasketProductDal : EntityRepositoryBase<BasketProduct, dbContext>, IBasketProductDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public BasketProductDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}