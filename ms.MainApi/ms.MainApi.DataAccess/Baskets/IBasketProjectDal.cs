using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels.Baskets;

namespace ms.MainApi.DataAccess.Baskets;

public interface IBasketProjectDal : IEntityRepository<BasketProject> { }

public class BasketProjectDal : EntityRepositoryBase<BasketProject, dbContext>, IBasketProjectDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public BasketProjectDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}