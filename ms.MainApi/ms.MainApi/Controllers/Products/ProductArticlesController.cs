using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Business.Cqrs.Products.ProductArticles;
using ms.MainApi.Entity.Models.Dtos.Products.ProductArticles;
using ms.MainApi.Entity.Models.Services;

namespace ms.MainApi.Controllers.Products;

[Route("api/[controller]")]
[ApiController]
public class ProductArticlesController : BaseController
{
    #region DI
    private readonly IMediator _mediator;

    public ProductArticlesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion


    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для получения Артикля по id:
    ///
    ///     GET api/ProductArticles/{1}
    ///
    /// </remarks>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBy([FromRoute] int id) =>
        Return(await _mediator.Send(new ProductArticleGetCommand(id)));

    /// <param name="catalogId"></param>
    /// <remarks>
    /// EndPoint для получения Артикля в разрезе catalogId:
    ///
    ///     GET api/ProductArticles/{1}
    ///
    /// </remarks>
    [HttpGet("GetByCatalogId/{catalogId}")]
    public async Task<IActionResult> GetByCatalogId([FromRoute] int catalogId) =>
        Return(await _mediator.Send(new ProductArticleGetListByCatalogIdCommand(catalogId)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для динамической фильтрации запросов:
    ///
    ///     POST /api/ProductArticles/list
    ///     {    
    ///       "search": "iPhone 14 pro",    //return record where [name, description] Contains "iphone 14 pro" with lower case
    ///       "query": {
    ///         "condition": "and",
    ///         "rules": [
    ///           {
    ///             "field": "name",
    ///             "operators": "in",
    ///             "type": "string",
    ///             "value": "",
    ///             "values": [
    ///               "iPhone 14 pro", "iPhone 15 pro"    //return record where name Contains ("iPhone 14 pro", "iPhone 15 pro")
    ///             ]
    ///           }
    ///         ]
    ///       },
    ///       "page": 1,
    ///       "pageSize": 10
    ///     }
    /// 
    /// </remarks>
    [HttpPost("list")]
    public async Task<IActionResult> GetList([FromBody] QueryPagination form) =>
        Return(await _mediator.Send(new ProductArticleGetListCommand(form)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для создания нового Артикля:
    ///
    ///     POST /api/ProductArticles
    ///     {
    ///        "name": "bergHoff",
    ///        "description": "Посуда",
    ///        "catalogId": 1
    ///     }
    /// </remarks>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] ProductArticleCreateDto form) =>
        Return(await _mediator.Send(new ProductArticleCreateCommand(form)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для редактирования Артикля по Id:
    ///
    ///     PUT /api/ProductArticles
    ///     {
    ///        "id": 1
    ///        "name": "bergHome",
    ///        "description": "Посуда",
    ///        "catalogId": 2
    ///     }
    /// </remarks>
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] ProductArticleUpdateDto form) =>
        Return(await _mediator.Send(new ProductArticleUpdateCommand(form)));

    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для удаления Артикля по id:
    ///
    ///     DELETE api/ProductArticles/{1}
    ///
    /// </remarks>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] int id) =>
        Return(await _mediator.Send(new ProductArticleDeleteCommand(id)));

}