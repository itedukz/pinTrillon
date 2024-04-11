using ms.MainApi.Core.DataAccess;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Contexts;
using ms.MainApi.Entity.Models.DbModels.Products;

namespace ms.MainApi.DataAccess.Products;

public interface IMaterialDal : IEntityRepository<Material> { }

public class MaterialDal : EntityRepositoryBase<Material, dbContext>, IMaterialDal
{
    private readonly IAuthInformationRepository _authInformationRepository;

    public MaterialDal(dbContext context, IAuthInformationRepository authInformationRepository) : base(context, authInformationRepository)
    {
        _authInformationRepository = authInformationRepository;
    }
}