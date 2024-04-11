using AutoMapper;
using MediatR;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.DataAccess.Organizations;
using ms.MainApi.Entity.Models.DbModels.Organizations;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.Dtos.Identities.Users;
using ms.MainApi.Entity.Models.Dtos.Organizations;
using ms.MainApi.Entity.Models.Dtos.Organizations.Cities;
using ms.MainApi.Entity.Models.Dtos.Pictures;

namespace ms.MainApi.Business.Cqrs.Organizations;

public class getOrganizationCommand : IRequest<OrganizationDto?>
{
    public int id { get; }

    public getOrganizationCommand(int _id)
    {
        id = _id;
    }

    public class Handler : IRequestHandler<getOrganizationCommand, OrganizationDto?>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IOrganizationDal _entityDal;
        private readonly IOrganizationPictureDal _bannerDal;
        private readonly IUserDal _userDal;
        private readonly ICityDal _cityDal;

        public Handler(IMapper mapper, IOrganizationDal entityDal, IOrganizationPictureDal bannerDal, IUserDal userDal, ICityDal cityDal)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _bannerDal = bannerDal;
            _userDal = userDal;
            _cityDal = cityDal;
        }
        #endregion

        public async Task<OrganizationDto?> Handle(getOrganizationCommand request, CancellationToken cancellationToken)
        {
            Organization? entity = await _entityDal.GetAsync(i => i.id == request.id);
            if (entity == null)
                return null;

            OrganizationDto entityDto = _mapper.Map<OrganizationDto>(entity);

            entityDto.city = _mapper.Map<CityDto>(await _cityDal.GetAsync(i => i.id == entity.cityId));
            entityDto.user = _mapper.Map<UserDto>(await _userDal.GetAsync(i => i.id == entity.userId));
            entityDto.banner = _mapper.Map<PictureDto>(await _bannerDal.GetAsync(i => i.organizationId == entity.id));

            return entityDto;
        }
    }
}