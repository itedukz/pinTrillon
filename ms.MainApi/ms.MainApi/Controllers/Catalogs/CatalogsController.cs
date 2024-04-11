using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Business.Cqrs.Catalogs;
using ms.MainApi.Entity.Models.Dtos.Catalogs;
using ms.MainApi.Entity.Models.Services;

namespace ms.MainApi.Controllers.Catalogs;

[Route("api/[controller]")]
[ApiController]

public class CatalogsController : BaseController
{
    #region DI
    private readonly IMediator _mediator;

    public CatalogsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для получения категории по идентификатору:
    ///
    ///     GET api/Catalogs/{1}
    ///
    /// </remarks>
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBy([FromRoute] int id) =>
        Return(await _mediator.Send(new CatalogGetCommand(id)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для динамической фильтрации запросов:
    ///
    ///     POST /api/Catalogs/list
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
    [AllowAnonymous]
    [HttpPost("list")]
    public async Task<IActionResult> GetList([FromBody] QueryPagination form) =>
        Return(await _mediator.Send(new CatalogGetListCommand(form)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для создания нового каталога:
    ///
    ///     POST /api/Catalogs
    ///     {
    ///        "name": "Book",
    ///        "description": "Раздел для книг"
    ///     }
    /// </remarks>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CatalogCreateDto form) =>
        Return(await _mediator.Send(new CatalogCreateCommand(form)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для редактирования категории по Id:
    ///
    ///     PUT /api/Catalogs
    ///     {
    ///        "id": 1
    ///        "name": "Books",
    ///        "description": "Books это раздел для книг"
    ///     }
    /// </remarks>
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] CatalogDto form) =>
        Return(await _mediator.Send(new CatalogUpdateCommand(form)));

    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для удаления категории по идентификатору:
    ///
    ///     DELETE api/Catalogs/{1}
    ///
    /// </remarks>
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id) =>
        Return(await _mediator.Send(new CatalogDeleteCommand(id)));

}