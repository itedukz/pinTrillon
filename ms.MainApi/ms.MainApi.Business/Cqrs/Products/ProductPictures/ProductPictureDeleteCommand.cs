using MediatR;
using ms.MainApi.Business.Services;
using ms.MainApi.DataAccess.Products;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using System.Net;

namespace ms.MainApi.Business.Cqrs.Products.ProductPictures;

public class ProductPictureDeleteCommand : IRequest<IMainResponseDto>
{
    public int productId { get; }
    public int pictureId { get; }

	public ProductPictureDeleteCommand(int _productId, int _pictureId)
	{
        productId = _productId;
        pictureId = _pictureId;
    }

    public class Handler : IRequestHandler<ProductPictureDeleteCommand, IMainResponseDto>
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

        public async Task<IMainResponseDto> Handle(ProductPictureDeleteCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.create, PermissionAction.update, PermissionAction.delete,
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.product, actions);

            if (!permission.isSuccess)
                return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            ProductPicture? entity = await _entityDal.GetAsync(i => i.id == request.pictureId && i.productId == request.productId);
            if (entity == null)
                return new MainResponseDto("ProductPicture not found", permission.permittedActions);

            try
            {
                string filePath = $"wwwroot{entity.FilePath}";
                var file = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                File.Delete(file);

                //entity.FilePath = "";
                //entity.FileName = "";
                //await _entityDal.UpdateAsync(entity);
                await _entityDal.DeleteAsync(entity);
            }
            catch {}

            return new MainResponseDto("ProductPicture is deleted");
        }
    }
}