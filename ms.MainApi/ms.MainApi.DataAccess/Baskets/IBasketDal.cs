using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels;

namespace ms.MainApi.DataAccess.Baskets;

internal interface IBasketDal : IEntityRepository<Basket> { }

public class BasketDal : EntityRepositoryBase<Basket, dbContext>, IBasketDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public BasketDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}