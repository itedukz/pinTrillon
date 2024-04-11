using FluentValidation;
using MediatR;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.Entity.Models.DbModels.Identity;
using ms.MainApi.Entity.Models.Dtos.Identities.Users;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Services;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Identities.Users;

public class UserUpdatePasswordCommand : IRequest<IMainResponseDto>
{
    public UserUpdatePasswordDto form { get; }

    public UserUpdatePasswordCommand(UserUpdatePasswordDto _form)
    {
        form = _form;
    }

    public class Handler : IRequestHandler<UserUpdatePasswordCommand, IMainResponseDto>
    {
        #region DI
        private readonly IUserDal _entityDal;
        private readonly IAuthInformationRepository _authInformationRepository;
        private readonly IValidator<UserUpdatePasswordCommand> _validator;
        private readonly IMessagesRepository _messagesRepository;

        public Handler(IUserDal entityDal, IAuthInformationRepository authInformationRepository,
            IValidator<UserUpdatePasswordCommand> validator, IMessagesRepository messagesRepository)
        {
            _entityDal = entityDal;
            _authInformationRepository = authInformationRepository;
            _validator = validator;
            _messagesRepository = messagesRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(UserUpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            #region Validation
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
                return new MainResponseDto(_messagesRepository.FormValidation(),
                    validation.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            #endregion

            User? entity = await _entityDal.GetAsync(i => i.id == _authInformationRepository.GetUserId());

            if (entity == null)
                return new MainResponseDto("Data no found");

            entity!.passwordHash = HashMD5.HashMD5String(request.form.password);
            await _entityDal.UpdateAsync(entity);

            return new MainResponseDto("Entity has been updated");
        }
    }
}