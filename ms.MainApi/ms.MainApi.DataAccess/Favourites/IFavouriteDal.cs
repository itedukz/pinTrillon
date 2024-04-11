using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels;

namespace ms.MainApi.DataAccess.Favourites;

internal interface IFavouriteDal : IEntityRepository<Favourite> { }

public class FavouriteDal : EntityRepositoryBase<Favourite, dbContext>, IFavouriteDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public FavouriteDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}