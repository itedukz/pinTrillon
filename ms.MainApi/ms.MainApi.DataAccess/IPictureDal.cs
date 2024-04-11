using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels;

namespace ms.MainApi.DataAccess;

public interface IPictureDal : IEntityRepository<Picture> { }

public class PictureDal : EntityRepositoryBase<Picture, dbContext>, IPictureDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public PictureDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}