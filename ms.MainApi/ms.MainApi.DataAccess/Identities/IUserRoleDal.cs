using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels.Identity;

namespace ms.MainApi.DataAccess.Identities;

public interface IUserRoleDal : IEntityRepository<UserRole> { }

public class UserRoleDal : EntityRepositoryBase<UserRole, dbContext>, IUserRoleDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public UserRoleDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}