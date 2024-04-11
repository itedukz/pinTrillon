using MediatR;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Baskets;
using ms.MainApi.Entity.Models.Dtos.Responses;

namespace ms.MainApi.Business.Cqrs.Baskets;

public class BasketRemoveProjectCommand : IRequest<IMainResponseDto>
{
    public int projectId { get; }

    public BasketRemoveProjectCommand(int _projectId)
    {
        projectId = _projectId;
    }

    public class Handler : IRequestHandler<BasketRemoveProjectCommand, IMainResponseDto>
    {
        #region DI
        private readonly IBasketProjectDal _basketDal;
        private readonly IAuthInformationRepository _authInformationRepository;

        public Handler(IBasketProjectDal basketDal, IAuthInformationRepository authInformationRepository)
        {
            _basketDal = basketDal;
            _authInformationRepository = authInformationRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(BasketRemoveProjectCommand request, CancellationToken cancellationToken)
        {
            int userId = _authInformationRepository.GetUserId();
            if (userId == 0)
                return new MainResponseDto("User not found");

            await _basketDal.DeleteAsync(i => i.userId == userId && i.projectId == request.projectId);

            return new MainResponseDto("Project removed from basket");
        }
    }
}