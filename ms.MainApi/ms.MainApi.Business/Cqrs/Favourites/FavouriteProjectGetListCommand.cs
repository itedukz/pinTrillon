using AutoMapper;
using MediatR;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Favourites;
using ms.MainApi.DataAccess.Projects;
using ms.MainApi.Entity.Models.DbModels.Favourites;
using ms.MainApi.Entity.Models.DbModels.Projects;
using ms.MainApi.Entity.Models.Dtos.Projects;
using ms.MainApi.Entity.Models.Dtos.Responses;

namespace ms.MainApi.Business.Cqrs.Favourites;

public class FavouriteProjectGetListCommand : IRequest<IMainResponseDto>
{
    public class Handler : IRequestHandler<FavouriteProjectGetListCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IFavouriteProjectDal _favouriteDal;
        private readonly IProjectDal _projectDal;
        private readonly IAuthInformationRepository _authInformationRepository;

        public Handler(IMapper mapper, IFavouriteProjectDal favouriteDal, IProjectDal projectDal,
            IAuthInformationRepository authInformationRepository)
        {
            _mapper = mapper;
            _favouriteDal = favouriteDal;
            _projectDal = projectDal;
            _authInformationRepository = authInformationRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(FavouriteProjectGetListCommand request, CancellationToken cancellationToken)
        {
            int userId = _authInformationRepository.GetUserId();
            if (userId == 0)
                return new MainResponseDto("User not found");

            List<FavouriteProject> entities = await _favouriteDal.GetAllAsync(i => i.userId == userId);
            if (entities == null || entities.Count == 0)
                return new MainResponseDto("Favourite not found");


            List<int> projectsId = entities.Select(e => e.projectId).ToList();
            List<Project> projects = await _projectDal.GetAllAsync(i => projectsId.Contains(i.id));
            List<ProjectShortDto> projectsDto = projects.Select(i => _mapper.Map<ProjectShortDto>(i)).ToList();

            return new MainResponseDto(projectsDto);
        }
    }
}