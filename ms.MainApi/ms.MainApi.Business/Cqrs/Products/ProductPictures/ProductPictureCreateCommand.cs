using MediatR;
using Microsoft.AspNetCore.Http;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers.RabbitMq;
using ms.MainApi.Core.GeneralHelpers.RabbitMq.Models;
using ms.MainApi.DataAccess.Products;
using ms.MainApi.Entity.Models.DbModels.Products;
using ms.MainApi.Entity.Models.Dtos.Responses;
using ms.MainApi.Entity.Models.Enums;
using ms.MainApi.Entity.Models.RabbitMq;
using Newtonsoft.Json;
using System.IO;

namespace ms.MainApi.Business.Cqrs.Products.ProductPictures;

public class ProductPictureCreateCommand : IRequest<IMainResponseDto>
{
    public int productId { get; }
    IFormFile? file { get; }
    public bool isMain { get; }

    public ProductPictureCreateCommand(int _productId, IFormFile? _file, bool _isMain)
    {
        productId = _productId;
        file = _file;
        isMain = _isMain;
    }

    public class Handler : IRequestHandler<ProductPictureCreateCommand, IMainResponseDto>
    {
        #region DI
        private readonly IRabbitMqHelperRepository _rabbitMqHelperRepository;
        private readonly IProductPictureDal _entityDal;
        private readonly IPermissionCheck _checkPermission;

        public Handler(IRabbitMqHelperRepository rabbitMqHelperRepository,
            IProductPictureDal entityDal, IPermissionCheck checkPermission)
        {
            _rabbitMqHelperRepository = rabbitMqHelperRepository;
            _entityDal = entityDal;
            _checkPermission = checkPermission;
        }
        #endregion

        public async Task<IMainResponseDto> Handle(ProductPictureCreateCommand request, CancellationToken cancellationToken)
        {
            #region checkPermission
            List<PermissionAction> actions = new List<PermissionAction>() {
                PermissionAction.create, PermissionAction.update, PermissionAction.delete,
            };
            var permission = await _checkPermission.CheckPermissionWithResponse(PermissionName.product, actions);

            //if (!permission.isSuccess)
            //    return new MainResponseDto(permission.message, HttpStatusCode.Forbidden);
            #endregion

            if (request.file == null || request.file.Length == 0)
                return new MainResponseDto("ProductPicture file is null");

            try
            {
                CheckFolder(request.productId.ToString());
                string path = "files/product/pictures/" + request.productId.ToString() + "/";
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.file.FileName).ToLower();
                var saveFilePath = Path.Combine(fullPath, fileName);

                await using var stream = new FileStream(saveFilePath, FileMode.Create);
                await request.file.CopyToAsync(stream, cancellationToken);


                List<ProductPicture>? entities = await _entityDal.GetAllAsync(i => i.productId == request.productId && i.isMain && request.isMain);
                foreach (ProductPicture item in entities)
                {
                    item.isMain = false;
                    await _entityDal.UpdateAsync(item);
                }

                ProductPicture entity = new ProductPicture
                {
                    productId = request.productId,
                    FileName = fileName,
                    FilePath = "/" + path + fileName,
                    isMain = request.isMain,
                    isProcessed = false,
                    picture = new byte[0]
                };

                long fileSize = request.file.Length;
                string fileType = request.file.ContentType;
                if (fileSize > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        request.file.CopyTo(memoryStream);
                        var bytes = memoryStream.ToArray();
                        entity.picture = bytes;
                    }
                }

                await _entityDal.AddAsync(entity);

                try
                {
                    _rabbitMqHelperRepository.CreateMessage(new RabbitMqSendRequestModel()
                    {
                        Factory = new RabbitMqConnectionFactoryModel
                        {
                            HostName = "82.200.245.234",                  //http://rmq.agrar.kz/  "rmq.agrar.kz"  82.200.245.234
                            Port = 5672,
                            UserName = "guest",
                            Password = "guest",
                        },
                        QueueInformation = new RabbitMqQueueInformationModel()
                        {
                            QueueName = "product-picture-sender-bg-service"
                        },
                        Message = JsonConvert.SerializeObject(new MessageNotificationDto
                        {
                            referenceId = entity.id,
                            MessageType = MessageTypes.productPicture
                        })
                    });
                }
                catch
                {
                    return new MainResponseDto("ProductPictures is saved with RabbitMq Producer Error");
                }

                return new MainResponseDto("ProductPictures is saved");
            }
            catch (Exception ex)
            {
                return new MainResponseDto(ex.Message);
            }
        }

        private void CheckFolder(string path)
        {
            var directoryPathList = new List<string>()
            {
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files"),
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "product"),
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "product", "pictures"),
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "product", "pictures", path)
            };
            foreach (var item in directoryPathList)
                if (!Directory.Exists(item))
                    Directory.CreateDirectory(item);
        }
    }
}