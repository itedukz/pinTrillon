using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Business.Cqrs.Products.Materials;
using ms.MainApi.Entity.Models.Dtos.Products.Materials;
using ms.MainApi.Entity.Models.Services;

namespace ms.MainApi.Controllers.Products;

[Route("api/[controller]")]
[ApiController]
public class MaterialsController : BaseController
{
    #region DI
    private readonly IMediator _mediator;

    public MaterialsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion


    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для получения Материяла по id:
    ///
    ///     GET api/Materials/{1}
    ///
    /// </remarks>
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBy([FromRoute] int id) =>
        Return(await _mediator.Send(new MaterialGetCommand(id)));


    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для динамической фильтрации запросов:
    ///
    ///     POST /api/Materials/list
    ///     {    
    ///       "search": "Дерево",    //return record where [name, description] Contains "Дерево" with lower case
    ///       "query": {
    ///         "condition": "and",
    ///         "rules": [
    ///           {
    ///             "field": "name",
    ///             "operators": "in",
    ///             "type": "string",
    ///             "value": "",
    ///             "values": [
    ///               "Дерево", "ПВХ"    //return record where name Contains ("Дерево", "ПВХ")
    ///             ]
    ///           }
    ///         ]
    ///       },
    ///       "page": 1,
    ///       "pageSize": 10
    ///     }
    /// 
    /// </remarks>
    [AllowAnonymous]
    [HttpPost("list")]
    public async Task<IActionResult> GetList([FromBody] QueryPagination form) =>
        Return(await _mediator.Send(new MaterialGetListCommand(form)));


    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для создания нового Материяла:
    ///
    ///     POST /api/Materials
    ///     {
    ///        "name": "ДСП",
    ///        "description": "Фанера"
    ///     }
    /// </remarks>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] MaterialCreateDto form) =>
        Return(await _mediator.Send(new MaterialCreateCommand(form)));


    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для редактирования Материяла по Id:
    ///
    ///     PUT /api/Materials
    ///     {
    ///        "id": 1
    ///        "name": "ЛДС",
    ///        "description": "Фанера"
    ///     }
    /// </remarks>
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] MaterialDto form) =>
        Return(await _mediator.Send(new MaterialUpdateCommand(form)));


    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для удаления Материяла по id:
    ///
    ///     DELETE api/Materials/{1}
    ///
    /// </remarks>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] int id) =>
        Return(await _mediator.Send(new MaterialDeleteCommand(id)));

}
