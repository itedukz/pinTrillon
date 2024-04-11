using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels.Identity;

namespace ms.MainApi.DataAccess.Identities;

public interface IRoleDal : IEntityRepository<Role> { }

public class RoleDal : EntityRepositoryBase<Role, dbContext>, IRoleDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public RoleDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}