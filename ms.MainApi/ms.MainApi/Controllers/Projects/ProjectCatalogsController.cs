using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Business.Cqrs.Projects.ProjectCatalogs;
using ms.MainApi.Entity.Models.Dtos.Projects.ProjectCatalogs;
using ms.MainApi.Entity.Models.Services;

namespace ms.MainApi.Controllers.Projects;

[Route("api/[controller]")]
[ApiController]
public class ProjectCatalogsController : BaseController
{
    #region DI
    private readonly IMediator _mediator;

    public ProjectCatalogsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для получения категории проекта по идентификатору:
    ///
    ///     GET api/ProjectCatalogs/{1}
    ///
    /// </remarks>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBy([FromRoute] int id) =>
        Return(await _mediator.Send(new ProjectCatalogGetCommand(id)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для динамической фильтрации запросов:
    ///
    ///     POST /api/ProjectCatalogs/list
    ///     {    
    ///       "search": "Детский",    //return record where [name, description] Contains "Book" with toLower
    ///       "query": {
    ///         "condition": "and",
    ///         "rules": [
    ///           {
    ///             "field": "name",
    ///             "operators": "in",
    ///             "type": "string",
    ///             "value": "",
    ///             "values": [
    ///               "Детский", "Кухня"    //return record where name Contains ("Детский", "Кухня")
    ///             ]
    ///           }
    ///         ]
    ///       },
    ///       "page": 1,        //Пагинация по страницам
    ///       "pageSize": 10
    ///     }
    /// 
    /// </remarks>
    [HttpPost("list")]
    public async Task<IActionResult> GetList([FromBody] QueryPagination form) =>
        Return(await _mediator.Send(new ProjectCatalogGetListCommand(form)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для создания новой категории проекта:
    ///
    ///     POST /api/ProjectCatalogs
    ///     {
    ///       "name": "Гостинная",
    ///       "description": "Раздел для проектов Гостинная"
    ///     }
    /// </remarks>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] ProjectCatalogCreateDto form) =>
        Return(await _mediator.Send(new ProjectCatalogCreateCommand(form)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для редактирования категории проекта по Id:
    ///
    ///     PUT /api/ProjectCatalogs
    ///     {
    ///       "id": 3,
    ///       "name": "Прихожая",
    ///       "description": "Раздел для проектов Прихожая"
    ///     }
    /// 
    /// </remarks>
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] ProjectCatalogUpdateDto form) =>
        Return(await _mediator.Send(new ProjectCatalogUpdateCommand(form)));

    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для удаления категории проекта по идентификатору:
    ///
    ///     DELETE api/ProjectCatalogs/{1}
    ///
    /// </remarks>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] int id) =>
        Return(await _mediator.Send(new ProjectCatalogDeleteCommand(id)));

}