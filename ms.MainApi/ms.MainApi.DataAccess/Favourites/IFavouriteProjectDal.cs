using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels.Favourites;

namespace ms.MainApi.DataAccess.Favourites;

public interface IFavouriteProjectDal : IEntityRepository<FavouriteProject> { }

public class FavouriteProjectDal : EntityRepositoryBase<FavouriteProject, dbContext>, IFavouriteProjectDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public FavouriteProjectDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}