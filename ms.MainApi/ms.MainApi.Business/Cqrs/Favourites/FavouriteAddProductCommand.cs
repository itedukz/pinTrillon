using FluentValidation;
using MediatR;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Favourites;
using ms.MainApi.Entity.Models.DbModels.Favourites;
using ms.MainApi.Entity.Models.Dtos.Responses;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Favourites;

public class FavouriteAddProductCommand : IRequest<IMainResponseDto>
{
    public int productId { get; }

    public FavouriteAddProductCommand(int _productId)
    {
        productId = _productId;
    }

    public class Handler : IRequestHandler<FavouriteAddProductCommand, IMainResponseDto>
    {
        #region DI
        private readonly IFavouriteProductDal _favouriteDal;
        private readonly IAuthInformationRepository _authInformationRepository;
        private readonly IValidator<FavouriteAddProductCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IFavouriteProductDal favouriteDal, IAuthInformationRepository authInformationRepository,
            IValidator<FavouriteAddProductCommand> validator, IMessagesRepository messagesRepository)
        {
            _favouriteDal = favouriteDal;
            _authInformationRepository = authInformationRepository;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(FavouriteAddProductCommand request, CancellationToken cancellationToken)
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

            if(_favouriteDal.Any(i=>i.userId == userId && i.productId == request.productId))
                return new MainResponseDto("Product added  to favourite");

            FavouriteProduct insertEntity = new FavouriteProduct
            {
                userId = userId,
                productId = request.productId
            };
            _favouriteDal.Add(insertEntity);

            return new MainResponseDto("Product added  to favourite");
        }
    }
}