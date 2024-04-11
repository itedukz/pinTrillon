using MediatR;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Favourites;
using ms.MainApi.Entity.Models.Dtos.Responses;

namespace ms.MainApi.Business.Cqrs.Favourites;

public class FavouriteRemoveProjectCommand : IRequest<IMainResponseDto>
{
    public int projectId { get; }

    public FavouriteRemoveProjectCommand(int _projectId)
    {
        projectId = _projectId;
    }

    public class Handler : IRequestHandler<FavouriteRemoveProjectCommand, IMainResponseDto>
    {
        #region DI
        private readonly IFavouriteProjectDal _favouriteDal;
        private readonly IAuthInformationRepository _authInformationRepository;

        public Handler(IFavouriteProjectDal favouriteDal, IAuthInformationRepository authInformationRepository)
        {
            _favouriteDal = favouriteDal;
            _authInformationRepository = authInformationRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(FavouriteRemoveProjectCommand request, CancellationToken cancellationToken)
        {
            int userId = _authInformationRepository.GetUserId();
            if (userId == 0)
                return new MainResponseDto("User not found");

            await _favouriteDal.DeleteAsync(i => i.userId == userId && i.projectId == request.projectId);

            return new MainResponseDto("Project removed from favourite");
        }
    }
}