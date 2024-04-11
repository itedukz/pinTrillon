using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.DataAccess.Products;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Products.ProductPictures;

public class ProductPictureSetMainCommand : IRequest<IMainResponseDto>
{
    public int pictureId { get; }

    public ProductPictureSetMainCommand(int _pictureId)
    {
        pictureId = _pictureId;
    }

    public class Handler : IRequestHandler<ProductPictureSetMainCommand, IMainResponseDto>
    {
        #region DI
        private readonly IProductPictureDal _entityDal;
        private readonly IPermissionCheck _checkPermission;

        public Handler(IProductPictureDal entityDal, IPermissionCheck checkPermission)
        {
            _entityDal = entityDal;
            _checkPermission = checkPermission;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(ProductPictureSetMainCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.create, PermissionAction.update, PermissionAction.delete,
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.product, actions);

            //if (!permission.isSuccess)
            //    return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            try
            {
                ProductPicture? entity = await _entityDal.GetAsync(i => i.id == request.pictureId);
                if (entity == null)
                    return new MainResponseDto("ProductPictures not found", HttpStatusCode.NotFound);

                List<ProductPicture>? entities = await _entityDal.GetAllAsync(i => i.productId == entity.productId && i.isMain);
                foreach (ProductPicture item in entities)
                {
                    item.isMain = false;
                    await _entityDal.UpdateAsync(item);
                }

                entity.isMain = true;
                await _entityDal.UpdateAsync(entity);

                return new MainResponseDto("ProductPictures isMain saved");
            }
            catch (Exception ex)
            {
                return new MainResponseDto(ex.Message);
            }
        }
    }
}