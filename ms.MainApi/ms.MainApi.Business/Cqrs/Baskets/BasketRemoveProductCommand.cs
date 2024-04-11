using MediatR;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Baskets;
using ms.MainApi.Entity.Models.Dtos.Responses;

namespace ms.MainApi.Business.Cqrs.Baskets;

public class BasketRemoveProductCommand : IRequest<IMainResponseDto>
{
    public int productId { get; }

    public BasketRemoveProductCommand(int _productId)
    {
        productId = _productId;
    }

    public class Handler : IRequestHandler<BasketRemoveProductCommand, IMainResponseDto>
    {
        #region DI
        private readonly IBasketProductDal _basketDal;
        private readonly IAuthInformationRepository _authInformationRepository;

        public Handler(IBasketProductDal basketDal, IAuthInformationRepository authInformationRepository)
        {
            _basketDal = basketDal;
            _authInformationRepository = authInformationRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(BasketRemoveProductCommand request, CancellationToken cancellationToken)
        {
            int userId = _authInformationRepository.GetUserId();
            if (userId == 0)
                return new MainResponseDto("User not found");

            await _basketDal.DeleteAsync(i => i.userId == userId && i.productId == request.productId);

            return new MainResponseDto("Product removed from basket");
        }
    }
}