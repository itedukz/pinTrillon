using AutoMapper;
using MediatR;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Baskets;
using ms.MainApi.DataAccess.Projects;
using ms.MainApi.Entity.Models.DbModels.Baskets;
using ms.MainApi.Entity.Models.DbModels.Projects;
using ms.MainApi.Entity.Models.Dtos.Projects;
using ms.MainApi.Entity.Models.Dtos.Responses;

namespace ms.MainApi.Business.Cqrs.Baskets;

public class BasketProjectGetListCommand : IRequest<IMainResponseDto>
{
    public class Handler : IRequestHandler<BasketProjectGetListCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly IBasketProjectDal _basketDal;
        private readonly IProjectDal _projectDal;
        private readonly IAuthInformationRepository _authInformationRepository;

        public Handler(IMapper mapper, IBasketProjectDal basketDal, IProjectDal projectDal,
            IAuthInformationRepository authInformationRepository)
        {
            _mapper = mapper;
            _basketDal = basketDal;
            _projectDal = projectDal;
            _authInformationRepository = authInformationRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(BasketProjectGetListCommand request, CancellationToken cancellationToken)
        {
            int userId = _authInformationRepository.GetUserId();
            if (userId == 0)
                return new MainResponseDto("User not found");

            List<BasketProject> entities = await _basketDal.GetAllAsync(i => i.userId == userId);
            if (entities == null || entities.Count == 0)
                return new MainResponseDto("Basket not found");

            List<int> projectsId = entities.Select(e => e.projectId).ToList();
            List<Project> projects = await _projectDal.GetAllAsync(i => projectsId.Contains(i.id));
            List<ProjectShortDto> projectsDto = projects.Select(i => _mapper.Map<ProjectShortDto>(i)).ToList();

            return new MainResponseDto(projectsDto);
        }
    }
}