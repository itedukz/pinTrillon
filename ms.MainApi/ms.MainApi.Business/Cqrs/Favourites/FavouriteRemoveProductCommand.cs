using MediatR;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Favourites;
using ms.MainApi.Entity.Models.Dtos.Responses;

namespace ms.MainApi.Business.Cqrs.Favourites;

public class FavouriteRemoveProductCommand : IRequest<IMainResponseDto>
{
    public int productId { get; }

    public FavouriteRemoveProductCommand(int _productId)
    {
        productId = _productId;
    }

    public class Handler : IRequestHandler<FavouriteRemoveProductCommand, IMainResponseDto>
    {
        #region DI
        private readonly IFavouriteProductDal _favouriteDal;
        private readonly IAuthInformationRepository _authInformationRepository;

        public Handler(IFavouriteProductDal favouriteDal, IAuthInformationRepository authInformationRepository)
        {
            _favouriteDal = favouriteDal;
            _authInformationRepository = authInformationRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(FavouriteRemoveProductCommand request, CancellationToken cancellationToken)
        {
            int userId = _authInformationRepository.GetUserId();
            if (userId == 0)
                return new MainResponseDto("User not found");

            await _favouriteDal.DeleteAsync(i => i.userId == userId && i.productId == request.productId);

            return new MainResponseDto("Product removed from favourite");
        }
    }
}