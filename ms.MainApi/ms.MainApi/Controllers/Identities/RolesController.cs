using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Business.Cqrs.Identities.Roles;
using ms.MainApi.Entity.Models.Dtos.Identities.Roles;
using ms.MainApi.Entity.Models.Services;

namespace ms.MainApi.Controllers.Identities;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RolesController : BaseController
{
    #region DI
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для получения роля по идентификатору:
    ///
    ///     GET api/Roles/{1}
    ///
    /// </remarks>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBy([FromRoute] int id) =>
        Return(await _mediator.Send(new RoleGetCommand(id)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для динамической фильтрации запросов:
    ///
    ///     POST /api/Roles/list
    ///     {    
    ///       "search": "Admin",    //return record where [name, description] Contains "Admin" with toLower
    ///       "query": {
    ///         "condition": "and",
    ///         "rules": [
    ///           {
    ///             "field": "name",
    ///             "operators": "in",
    ///             "type": "string",
    ///             "value": "",
    ///             "values": [
    ///               "Admin", "Manager"    //return record where name Contains ("Admin", "Manager")
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
        Return(await _mediator.Send(new RoleGetListCommand(form)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для создания нового роля:
    ///
    ///     POST /api/Roles
    ///     {
    ///        "name": "Admin",
    ///        "description": "Роль с привелегиями Администратора"
    ///     }
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoleCreateDto form) =>
        Return(await _mediator.Send(new RoleCreateCommand(form)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для редактирования роля по Id:
    ///
    ///     PUT /api/Roles
    ///     {
    ///        "id": 1
    ///        "name": "Админ",
    ///        "description": "Роль с привелегиями Администратора"
    ///     }
    /// </remarks>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] RoleUpdateDto form) =>
        Return(await _mediator.Send(new RoleUpdateCommand(form)));

    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для удаления роля по идентификатору:
    ///
    ///     DELETE api/Roles/{1}
    ///
    /// </remarks>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id) =>
        Return(await _mediator.Send(new RoleDeleteCommand(id)));

}