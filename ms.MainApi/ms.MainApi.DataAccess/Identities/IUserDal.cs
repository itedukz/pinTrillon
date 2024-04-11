using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels.Identity;

namespace ms.MainApi.DataAccess.Identities;

public interface IUserDal : IEntityRepository<User> { }


public class UserDal : EntityRepositoryBase<User, dbContext>, IUserDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public UserDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}