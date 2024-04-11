using FluentValidation;
using MediatR;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Baskets;
using ms.MainApi.Entity.Models.DbModels.Baskets;
using ms.MainApi.Entity.Models.Dtos.Responses;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Baskets;

public class BasketAddProductCommand : IRequest<IMainResponseDto>
{
    public int productId { get; }

    public BasketAddProductCommand(int _productId)
    {
        productId = _productId;
    }

    public class Handler : IRequestHandler<BasketAddProductCommand, IMainResponseDto>
    {
        #region DI
        private readonly IBasketProductDal _basketDal;
        private readonly IAuthInformationRepository _authInformationRepository;
        private readonly IValidator<BasketAddProductCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IBasketProductDal basketDal, IAuthInformationRepository authInformationRepository,
            IValidator<BasketAddProductCommand> validator, IMessagesRepository messagesRepository)
        {
            _basketDal = basketDal;
            _authInformationRepository = authInformationRepository;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(BasketAddProductCommand request, CancellationToken cancellationToken)
        {
            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            int userId = _authInformationRepository.GetUserId();
            if (userId == 0)
                return new MainResponseDto("User not found");

            if (_basketDal.Any(i => i.userId == userId && i.productId == request.productId))
                return new MainResponseDto("Product added  to basket");

            BasketProduct insertEntity = new BasketProduct
            {
                userId = userId,
                productId = request.productId
            };
            _basketDal.Add(insertEntity);

            return new MainResponseDto("Product added  to basket");
        }
    }
}