using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Business.Cqrs.Products.ProductPictures;
using ms.MainApi.Business.Cqrs.Projects;
using ms.MainApi.Business.Cqrs.Projects.ProjectPictures;
using ms.MainApi.Entity.Models.DbModels.Projects;
using ms.MainApi.Entity.Models.Dtos.Projects;
using ms.MainApi.Entity.Models.Services;

namespace ms.MainApi.Controllers.Projects;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : BaseController
{
    #region DI
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    #region Getter

    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для получения проекта по идентификатору:
    ///
    ///     GET api/Projects/{1}
    ///
    /// </remarks>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBy([FromRoute] int id) =>
        Return(await _mediator.Send(new ProjectGetCommand(id)));


    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для динамической фильтрации запросов:
    ///
    ///     POST /api/Projects/list
    ///     {    
    ///       "search": "Children's white",    //return record where [name, description] Contains "Children's white" with toLower
    ///       "query": {
    ///         "condition": "and",
    ///         "rules": [
    ///           {
    ///             "field": "name",
    ///             "operators": "in",
    ///             "type": "string",
    ///             "value": "",
    ///             "values": [
    ///               "Children's white", "Children's blue"    //return record where name Contains ("Children's white", "Children's blue")
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
        Return(await _mediator.Send(new ProjectGetListCommand(form)));

    #endregion


    #region Create, Update, Delete

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для создания нового проекта:
    ///
    ///     POST /api/Projects
    ///     {
    ///       "name": "Детская Boys Blue",
    ///       "article": "Boys Blue",
    ///       "description": "Детская Boys Blue описание",
    ///       "price": 15000,
    ///       "quadrature": 25,
    ///       "measure": {
    ///         "height": 3,
    ///         "width": 4,
    ///         "length": 5
    ///       },
    ///       "colorsId": [
    ///         2, 3
    ///       ],
    ///       "projectCatalogId": 1
    ///     }
    ///     
    /// </remarks>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] ProjectCreateDto form) =>
        Return(await _mediator.Send(new ProjectCreateCommand(form)));


    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для редактирования проекта по Id:
    ///
    ///     PUT /api/Projects
    ///     {
    ///       "id": 1,
    ///       "name": "Детская Boys White",
    ///       "article": "Boys Blue",
    ///       "description": "Детская Boys White описание",
    ///       "price": 15500,
    ///       "quadrature": 25,
    ///       "measure": {
    ///         "height": 3,
    ///         "width": 5,
    ///         "length": 4
    ///       },
    ///       "colorsId": [
    ///         2, 3
    ///       ],
    ///       "projectCatalogId": 1
    ///     }
    /// 
    /// </remarks>
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] ProjectUpdateDto form) =>
        Return(await _mediator.Send(new ProjectUpdateCommand(form)));


    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для удаления проекта по идентификатору:
    ///
    ///     DELETE api/Projects/{1}
    ///
    /// </remarks>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] int id) =>
        Return(await _mediator.Send(new ProjectDeleteCommand(id)));

    #endregion


    #region фото проекта (Upload, PictureDelete)

    /// <param name="projectId"></param>
    /// <param name="isMain"></param>
    /// <param name="avatar"></param>
    /// <remarks>
    /// EndPoint для загрузки фото проекта. Отправить через [FromForm] массив IFormFile file
    ///
    ///     POST /api/Projects/picture/{1}
    ///     {
    ///        "avatar": "input Type = 'file', name = 'avatar'"
    ///     }
    /// </remarks>
    [RequestSizeLimit(1024 * 1024 * 5)]
    [HttpPost("picture/{projectId}")]
    public async Task<IActionResult> Upload([FromRoute] int projectId, [FromForm] bool isMain, IFormFile? avatar) =>
        Return(await _mediator.Send(new ProjectPictureCreateCommand(projectId, avatar, isMain)));


    /// <param name="pictureId"></param>
    /// <remarks>
    /// EndPoint для того, чтобы сделать Картинку главным по идентификатору самой картинки:
    ///
    ///     GET api/Products/pictureSetMain/{1}
    ///
    /// </remarks>
    [HttpGet("pictureSetMain/{pictureId}")]
    public async Task<IActionResult> PictureSetMain([FromRoute] int pictureId) =>
        Return(await _mediator.Send(new ProjectPictureSetMainCommand(pictureId)));


    /// <param name="projectId"></param>
    /// <param name="pictureId"></param>
    /// <remarks>
    /// EndPoint для удаления картинки по идентификатору проекта и Картинки:
    ///
    ///     DELETE api/Products/pictureDelete/{1}/{1}
    ///
    /// </remarks>
    [HttpDelete("pictureDelete/{projectId}/{pictureId}")]
    [Authorize]
    public async Task<IActionResult> PictureDelete([FromRoute] int projectId, [FromRoute] int pictureId) =>
        Return(await _mediator.Send(new ProjectPictureDeleteCommand(projectId, pictureId)));

    #endregion


    #region Планировка проекта (Upload, layoutDelete)

    /// <param name="projectId"></param>
    /// <param name="avatar"></param>
    /// <remarks>
    /// EndPoint для загрузки Планировки проекта. Отправить через [FromForm] массив IFormFile file
    ///
    ///     POST /api/Projects/layout/{1}
    ///     {
    ///        "avatar": "input Type = 'file', name = 'avatar'"
    ///     }
    /// </remarks>
    [RequestSizeLimit(1024 * 1024 * 5)]
    [HttpPost("layout/{projectId}")]
    public async Task<IActionResult> layoutUpload([FromRoute] int projectId, [FromForm] IFormFile? avatar) =>
        Return(await _mediator.Send(new ProjectLayoutCreateCommand(projectId, avatar)));


    /// <param name="projectId"></param>
    /// <remarks>
    /// EndPoint для удаления Планировки по идентификатору проекта:
    ///
    ///     DELETE api/Products/layoutDelete/{1}/{1}
    ///
    /// </remarks>
    [HttpDelete("layoutDelete/{projectId}")]
    [Authorize]
    public async Task<IActionResult> layoutDelete([FromRoute] int projectId) =>
        Return(await _mediator.Send(new ProjectLayoutDeleteCommand(projectId)));

    #endregion

}