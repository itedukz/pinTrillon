using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels.Identity;

namespace ms.MainApi.DataAccess.Identities;

public interface IPermissionDal : IEntityRepository<Permission> { }


public class PermissionDal : EntityRepositoryBase<Permission, dbContext>, IPermissionDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public PermissionDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}