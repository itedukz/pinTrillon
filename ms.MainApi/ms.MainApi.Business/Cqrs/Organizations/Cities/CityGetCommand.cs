using AutoMapper;
using FluentValidation;
using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Organizations;
using ms.MainApi.Entity.Models.DbModels.Organizations;
using ms.MainApi.Entity.Models.Dtos.Organizations.Cities;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Organizations.Cities;

public class CityGetCommand : IRequest<IMainResponseDto>
{
    public int id { get; }

    public CityGetCommand(int _id)
    {
        id = _id;
    }

    public class Handler : IRequestHandler<CityGetCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly ICityDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<CityGetCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IMapper mapper, ICityDal entityDal, IPermissionCheck checkPermission,
            IValidator<CityGetCommand> validator, IMessagesRepository messagesRepository)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(CityGetCommand request, CancellationToken cancellationToken)
        {
            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            #region checkPermission
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.city);
            #endregion

            City? entity = await _entityDal.GetAsync(i => i.id == request.id);

            return new MainResponseDto(_mapper.Map<CityDto>(entity), permission.permittedActions);
        }
    }
}