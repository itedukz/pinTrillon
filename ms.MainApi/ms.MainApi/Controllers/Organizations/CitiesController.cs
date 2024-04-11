using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Business.Cqrs.Organizations.Cities;
using ms.MainApi.Entity.Models.Dtos.Organizations.Cities;
using ms.MainApi.Entity.Models.Services;

namespace ms.MainApi.Controllers.Organizations;

[Route("api/[controller]")]
[ApiController]
public class CitiesController : BaseController
{
    #region DI
    private readonly IMediator _mediator;

    public CitiesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для получения Города по id:
    ///
    ///     GET api/Cities/{1}
    ///
    /// </remarks>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBy([FromRoute] int id) =>
        Return(await _mediator.Send(new CityGetCommand(id)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для динамической фильтрации запросов:
    ///
    ///     POST /api/Cities/list
    ///     {    
    ///       "search": "Алматы",    //return record where [name, description] Contains "алматы" with lower case
    ///       "query": {
    ///         "condition": "and",
    ///         "rules": [
    ///           {
    ///             "field": "name",
    ///             "operators": "in",
    ///             "type": "string",
    ///             "value": "",
    ///             "values": [
    ///               "Алматы", "Астана"    //return record where name Contains ("Алматы", "Астана")
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
        Return(await _mediator.Send(new CityGetListCommand(form)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для создания нового города:
    ///
    ///     POST /api/Cities
    ///     {
    ///        "name": "Түркістан",
    ///        "description": "Облыс орталығы"
    ///     }
    /// </remarks>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CityCreateDto form) =>
        Return(await _mediator.Send(new CityCreateCommand(form)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для редактирования города по Id:
    ///
    ///     PUT /api/Cities
    ///     {
    ///        "id": 1
    ///        "name": "Туркестан",
    ///        "description": "Обласной центр"
    ///     }
    /// </remarks>
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] CityUpdateDto form) =>
        Return(await _mediator.Send(new CityUpdateCommand(form)));

    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для удаления города по id:
    ///
    ///     DELETE api/Cities/{1}
    ///
    /// </remarks>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] int id) =>
        Return(await _mediator.Send(new CityDeleteCommand(id)));

}