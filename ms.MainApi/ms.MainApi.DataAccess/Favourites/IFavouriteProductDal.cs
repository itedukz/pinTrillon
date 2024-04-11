using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels.Favourites;

namespace ms.MainApi.DataAccess.Favourites;

public interface IFavouriteProductDal : IEntityRepository<FavouriteProduct> { }

public class FavouriteProductDal : EntityRepositoryBase<FavouriteProduct, dbContext>, IFavouriteProductDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public FavouriteProductDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}