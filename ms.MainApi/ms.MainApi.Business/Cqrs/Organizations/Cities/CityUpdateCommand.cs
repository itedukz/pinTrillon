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

public class CityUpdateCommand : IRequest<IMainResponseDto>
{
    public CityUpdateDto form { get; }

    public CityUpdateCommand(CityUpdateDto _form)
    {
        form = _form;
    }

    public class Handler : IRequestHandler<CityUpdateCommand, IMainResponseDto>
    {
        #region DI
        private readonly IMapper _mapper;
        private readonly ICityDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IValidator<CityUpdateCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IMapper mapper, ICityDal entityDal, IPermissionCheck checkPermission,
            IValidator<CityUpdateCommand> validator, IMessagesRepository messagesRepository)
        {
            _mapper = mapper;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(CityUpdateCommand request, CancellationToken cancellationToken)
        {
            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.update
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.city, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            City entity = _mapper.Map<City>(request.form);
            await _entityDal.UpdateAsync(entity);

            return new MainResponseDto(_mapper.Map<CityDto>(entity), permission.permittedActions);
        }
    }
}