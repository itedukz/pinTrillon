using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Business.Cqrs.Products;
using ms.MainApi.Business.Cqrs.Products.ProductPictures;
using ms.MainApi.Entity.Models.DbModels;
using ms.MainApi.Entity.Models.Dtos.Products;
using ms.MainApi.Entity.Models.Services;

namespace ms.MainApi.Controllers.Products;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : BaseController
{
    #region DI
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для получения товара по идентификатору:
    ///
    ///     GET api/Products/{1}
    ///
    /// </remarks>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBy([FromRoute] int id) =>
        Return(await _mediator.Send(new ProductGetCommand(id)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для динамической фильтрации запросов:
    ///
    ///     POST /api/Products/list
    ///     {    
    ///       "search": "Book",    //return record where [name, description] Contains "Book" with toLower
    ///       "query": {
    ///         "condition": "and",
    ///         "rules": [
    ///           {
    ///             "field": "name",
    ///             "operators": "in",
    ///             "type": "string",
    ///             "value": "",
    ///             "values": [
    ///               "Book", "Note"    //return record where name Contains ("Book", "Note")
    ///             ]
    ///           }
    ///         ]
    ///       },
    ///       "page": 1,        //Пагинация по страницам
    ///       "pageSize": 10
    ///     }
    /// 
    /// 
    /// </remarks>
    [HttpPost("list")]
    public async Task<IActionResult> GetList([FromBody] QueryPagination form) =>
        Return(await _mediator.Send(new ProductGetListCommand(form)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для создания нового Товара:
    ///
    ///     POST /api/Products
    ///     {
    ///       "name": "iPhone 14 pro max",
    ///       "description": "iPhone 14 pro max siroco",
    ///       "price": 500000,
    ///       "catalogId": 0,
    ///       "productArticleId": 1,
    ///       "brandId": 1,
    ///       "materialsId": [
    ///         1
    ///       ],
    ///       "colorsId": [
    ///         0
    ///       ],
    ///       "measure": {
    ///         "height": 150,
    ///         "width": 100,
    ///         "length": 10,
    ///         "measureType": 1
    ///       }
    ///     }
    /// </remarks>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto form) =>
        Return(await _mediator.Send(new ProductCreateCommand(form)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для редактирования товара по Id:
    ///
    ///     PUT /api/Products
    ///     {
    ///       "id": 3,
    ///       "name": "iPhone 14 pro max",
    ///       "description": "iPhone 14 pro max siroco",
    ///       "price": 500000,
    ///       "catalogId": 0,
    ///       "productArticleId": 1,
    ///       "brandId": 1,
    ///       "materialsId": [
    ///         1
    ///       ],
    ///       "colorsId": [
    ///         0
    ///       ],
    ///       "measure": {
    ///         "height": 150,
    ///         "width": 100,
    ///         "length": 10,
    ///         "measureType": 1
    ///       }
    ///     }
    /// 
    /// </remarks>
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] ProductUpdateDto form) =>
        Return(await _mediator.Send(new ProductUpdateCommand(form)));

    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для удаления товара по идентификатору:
    ///
    ///     DELETE api/Products/{1}
    ///
    /// </remarks>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] int id) =>
        Return(await _mediator.Send(new ProductDeleteCommand(id)));


    #region фото товара (Upload, PictureDelete)

    /// <param name="productId"></param>
    /// <param name="isMain"></param>
    /// <param name="avatar"></param>
    /// <remarks>
    /// EndPoint для загрузки фото товара. Отправить через [FromForm] массив IFormFile avatar
    ///
    ///     POST /api/Products/picture/{1}
    ///     {
    ///        "avatar": "input Type = 'file', name = 'avatar'"
    ///     }
    /// </remarks>
    [RequestSizeLimit(1024 * 1024 * 5)]
    [HttpPost("picture/{productId}")]
    public async Task<IActionResult> Upload([FromRoute] int productId, [FromForm] bool isMain, IFormFile? avatar) =>
        Return(await _mediator.Send(new ProductPictureCreateCommand(productId, avatar, isMain)));


    /// <param name="pictureId"></param>
    /// <remarks>
    /// EndPoint для того, чтобы сделать Картинку главным по идентификатору самой картинки:
    ///
    ///     GET api/Products/pictureSetMain/{1}
    ///
    /// </remarks>
    [HttpGet("pictureSetMain/{pictureId}")]
    public async Task<IActionResult> PictureSetMain([FromRoute] int pictureId) =>
        Return(await _mediator.Send(new ProductPictureSetMainCommand(pictureId)));


    /// <param name="productId"></param>
    /// <param name="pictureId"></param>
    /// <remarks>
    /// EndPoint для удаления картинки по идентификатору продукта и картинки:
    ///
    ///     DELETE api/Products/pictureDelete/{1}/{1}
    ///
    /// </remarks>
    [HttpDelete("pictureDelete/{productId}/{pictureId}")]
    [Authorize]
    public async Task<IActionResult> PictureDelete([FromRoute] int productId, [FromRoute] int pictureId) =>
        Return(await _mediator.Send(new ProductPictureDeleteCommand(productId, pictureId)));

    #endregion
}