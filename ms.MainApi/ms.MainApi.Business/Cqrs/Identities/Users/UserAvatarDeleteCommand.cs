using MediatR;
using ms.MainApi.Business.Cqrs.Products.ProductPictures;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.DataAccess.Identities;
using ms.MainApi.DataAccess.Products;
using ms.MainApi.Entity.Models.DbModels.Identity;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Identities.Users;

public class UserAvatarDeleteCommand : IRequest<IMainResponseDto>
{
    public int? userId { get; }

    public UserAvatarDeleteCommand(int? _userId)
    {
        userId = _userId;
    }

    public class Handler : IRequestHandler<UserAvatarDeleteCommand, IMainResponseDto>
    {
        #region DI
        private readonly IUserDal _entityDal;
        private readonly IPermissionCheck _checkPermission;
        private readonly IAuthInformationRepository _authInformationRepository;

        public Handler(IUserDal entityDal, IPermissionCheck checkPermission, IAuthInformationRepository authInformationRepository)
        {
            _entityDal = entityDal;
            _checkPermission = checkPermission;
            _authInformationRepository = authInformationRepository;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(UserAvatarDeleteCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.create, PermissionAction.update, PermissionAction.delete,
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.user, actions);

            if (!permission.isSuccess && request.userId != _authInformationRepository.GetUserId())
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            int userId = request.userId ?? _authInformationRepository.GetUserId();

            User? entity = await _entityDal.GetAsync(i => i.id == userId);
            if (entity == null)
                return new MainResponseDto("User not found", permission.permittedActions);

            try
            {
                string filePath = $"wwwroot{entity.filePath}";
                var file = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                File.Delete(file);

                entity.filePath = "";
                entity.fileName = "";
                await _entityDal.UpdateAsync(entity);
            }
            catch { }

            return new MainResponseDto("User avatar is deleted");
        }
    }
}