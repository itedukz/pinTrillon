using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Business.Cqrs.Products.Brands;
using ms.MainApi.Entity.Models.Dtos.Products.Brands;
using ms.MainApi.Entity.Models.Services;

namespace ms.MainApi.Controllers.Products;

[Route("api/[controller]")]
[ApiController]
public class BrandsController : BaseController
{
    #region DI
    private readonly IMediator _mediator;

    public BrandsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion


    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для получения Бренда по id:
    ///
    ///     GET api/Brands/{1}
    ///
    /// </remarks>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBy([FromRoute] int id) =>
        Return(await _mediator.Send(new BrandGetCommand(id)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для динамической фильтрации запросов:
    ///
    ///     POST /api/Brands/list
    ///     {    
    ///       "search": "Apple",    //return record where [name, description] Contains "apple" with lower case
    ///       "query": {
    ///         "condition": "and",
    ///         "rules": [
    ///           {
    ///             "field": "name",
    ///             "operators": "in",
    ///             "type": "string",
    ///             "value": "",
    ///             "values": [
    ///               "iPhone", "MacBook"    //return record where name Contains ("iPhone", "MacBook")
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
        Return(await _mediator.Send(new BrandGetListCommand(form)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для создания нового бренда:
    ///
    ///     POST /api/Brands
    ///     {
    ///        "name": "bergHoff",
    ///        "description": "Посуда"
    ///     }
    /// </remarks>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] BrandCreateDto form) =>
        Return(await _mediator.Send(new BrandCreateCommand(form)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для редактирования бренда по Id:
    ///
    ///     PUT /api/Brands
    ///     {
    ///        "id": 1
    ///        "name": "bergHome",
    ///        "description": "Посуда"
    ///     }
    /// </remarks>
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] BrandDto form) =>
        Return(await _mediator.Send(new BrandUpdateCommand(form)));

    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для удаления бренда по id:
    ///
    ///     DELETE api/Brands/{1}
    ///
    /// </remarks>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] int id) =>
        Return(await _mediator.Send(new BrandDeleteCommand(id)));

}