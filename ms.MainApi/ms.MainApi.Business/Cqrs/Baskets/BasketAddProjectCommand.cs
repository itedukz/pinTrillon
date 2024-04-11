using FluentValidation;
using MediatR;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Baskets;
using ms.MainApi.Entity.Models.DbModels.Baskets;
using ms.MainApi.Entity.Models.Dtos.Responses;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Baskets;

public class BasketAddProjectCommand : IRequest<IMainResponseDto>
{
    public int projectId { get; }

    public BasketAddProjectCommand(int _projectId)
    {
        projectId = _projectId;
    }

    public class Handler : IRequestHandler<BasketAddProjectCommand, IMainResponseDto>
    {
        #region DI
        private readonly IBasketProjectDal _basketDal;
        private readonly IAuthInformationRepository _authInformationRepository;
        private readonly IValidator<BasketAddProjectCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IBasketProjectDal basketDal, IAuthInformationRepository authInformationRepository,
            IValidator<BasketAddProjectCommand> validator, IMessagesRepository messagesRepository)
        {
            _basketDal = basketDal;
            _authInformationRepository = authInformationRepository;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(BasketAddProjectCommand request, CancellationToken cancellationToken)
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

            if (_basketDal.Any(i => i.userId == userId && i.projectId == request.projectId))
                return new MainResponseDto("Project added to basket");

            BasketProject insertEntity = new BasketProject
            {
                userId = userId,
                projectId = request.projectId,
            };
            _basketDal.Add(insertEntity);

            return new MainResponseDto("Project added to basket");
        }
    }
}