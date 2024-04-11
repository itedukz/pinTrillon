using FluentValidation;
using MediatR;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Favourites;
using ms.MainApi.Entity.Models.DbModels.Favourites;
using ms.MainApi.Entity.Models.Dtos.Responses;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Favourites;

public class FavouriteAddProjectCommand : IRequest<IMainResponseDto>
{
    public int projectId { get; }

    public FavouriteAddProjectCommand(int _projectId)
    {
        projectId = _projectId;
    }

    public class Handler : IRequestHandler<FavouriteAddProjectCommand, IMainResponseDto>
    {
        #region DI
        private readonly IFavouriteProjectDal _favouriteDal;
        private readonly IAuthInformationRepository _authInformationRepository;
        private readonly IValidator<FavouriteAddProjectCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IFavouriteProjectDal favouriteDal, IAuthInformationRepository authInformationRepository,
            IValidator<FavouriteAddProjectCommand> validator, IMessagesRepository messagesRepository)
        {
            _favouriteDal = favouriteDal;
            _authInformationRepository = authInformationRepository;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(FavouriteAddProjectCommand request, CancellationToken cancellationToken)
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

            if (_favouriteDal.Any(i => i.userId == userId && i.projectId == request.projectId))
                return new MainResponseDto("Project added to favourite");

            FavouriteProject insertEntity = new FavouriteProject
            {
                userId = userId,
                projectId = request.projectId,
            };
            _favouriteDal.Add(insertEntity);

            return new MainResponseDto("Project added to favourite");
        }
    }
}