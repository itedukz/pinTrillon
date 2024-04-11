using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Business.Cqrs.Organizations;
using ms.MainApi.Business.Cqrs.Organizations.OrganizationPictures;
using ms.MainApi.Business.Cqrs.Projects.ProjectPictures;
using ms.MainApi.Entity.Models.DbModels.Organizations;
using ms.MainApi.Entity.Models.Dtos.Organizations;
using ms.MainApi.Entity.Models.Services;

namespace ms.MainApi.Controllers.Organizations;

[Route("api/[controller]")]
[ApiController]
public class OrganizationsController : BaseController
{
    #region DI
    private readonly IMediator _mediator;

    public OrganizationsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    #region Getter

    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для получения Организации по id:
    ///
    ///     GET api/Organizations/{1}
    ///
    /// </remarks>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBy([FromRoute] int id) =>
        Return(await _mediator.Send(new OrganizationGetCommand(id)));


    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для динамической фильтрации запросов:
    ///
    ///     POST /api/Organizations/list
    ///     {    
    ///       "search": "12 месяцев",    //return record where [name, description] Contains "12 месяцев" with lower case
    ///       "query": {
    ///         "condition": "and",
    ///         "rules": [
    ///           {
    ///             "field": "name",
    ///             "operators": "in",
    ///             "type": "string",
    ///             "value": "",
    ///             "values": [
    ///               "7 құрдас", "бақорда"    //return record where name Contains ("7 құрдас", "бақорда")
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
        Return(await _mediator.Send(new OrganizationGetListCommand(form)));

    #endregion


    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для создания новой организации:
    ///
    ///     POST /api/Organizations
    ///     {
    ///       "name": "itWay.kz",
    ///       "description": "Курсы по программированию",
    ///       "phoneNumber": [
    ///         "+7 747 2203400"
    ///       ],
    ///       "cityId": 1,
    ///       "regDate": "2024-03-15T13:05:25.022Z",
    ///       "isActive": true
    ///     }
    /// 
    /// </remarks>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] OrganizationCreateDto form) =>
        Return(await _mediator.Send(new OrganizationCreateCommand(form)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для редактирования организации по Id:
    ///
    ///     PUT /api/Organizations
    ///    { 
    ///      "id": 1,
    ///      "name": "itWay.kz",
    ///      "description": "Программирование курсы",
    ///      "phoneNumber": [
    ///        "+7 747 2203400"
    ///      ],
    ///      "cityId": 1,
    ///      "regDate": "2024-03-15T13:05:25.022Z",
    ///      "isActive": true
    ///    }
    /// 
    /// </remarks>
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] OrganizationUpdateDto form) =>
        Return(await _mediator.Send(new OrganizationUpdateCommand(form)));

    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для удаления организации по id:
    ///
    ///     DELETE api/Organizations/{1}
    ///
    /// </remarks>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] int id) =>
        Return(await _mediator.Send(new OrganizationDeleteCommand(id)));


    #region фото проекта (Upload, PictureDelete)

    /// <param name="organizationId"></param>
    /// <param name="avatar"></param>
    /// <remarks>
    /// EndPoint для загрузки Баннера. Отправить через [FromForm] массив IFormFile avatar
    ///
    ///     POST /api/Projects/banner/{1}
    ///     {
    ///        "avatar": "input Type = 'file', name = 'avatar'"
    ///     }
    /// </remarks>
    [RequestSizeLimit(1024 * 1024 * 5)]
    [HttpPost("banner/{organizationId}")]
    public async Task<IActionResult> BannerUpload([FromRoute] int organizationId, [FromForm] IFormFile? avatar) =>
        Return(await _mediator.Send(new OrganizationBannerCreateCommand(organizationId, avatar)));


    /// <param name="organizationId"></param>
    /// <remarks>
    /// EndPoint для удаления Баннера по идентификатору:
    ///
    ///     DELETE api/Products/bannerDelete/{1}/{1}
    ///
    /// </remarks>
    [HttpDelete("bannerDelete/{organizationId}")]
    [Authorize]
    public async Task<IActionResult> BannerDelete([FromRoute] int organizationId) =>
        Return(await _mediator.Send(new OrganizationBannerDeleteCommand(organizationId)));



    /// <param name="organizationId"></param>
    /// <param name="avatar"></param>
    /// <remarks>
    /// EndPoint для загрузки Аватара. Отправить через [FromForm] массив IFormFile avatar
    ///
    ///     POST /api/Projects/avatar/{1}
    ///     {
    ///        "avatar": "input Type = 'file', name = 'avatar'"
    ///     }
    /// </remarks>
    [RequestSizeLimit(1024 * 1024 * 5)]
    [HttpPost("avatar/{organizationId}")]
    public async Task<IActionResult> AvatarUpload([FromRoute] int organizationId, [FromForm] IFormFile? avatar) =>
        Return(await _mediator.Send(new OrganizationAvatarCreateCommand(organizationId, avatar)));


    /// <param name="organizationId"></param>
    /// <remarks>
    /// EndPoint для удаления Аватара по идентификатору:
    ///
    ///     DELETE api/Products/avatarDelete/{1}/{1}
    ///
    /// </remarks>
    [HttpDelete("avatarDelete/{organizationId}")]
    [Authorize]
    public async Task<IActionResult> AvatarDelete([FromRoute] int organizationId) =>
        Return(await _mediator.Send(new OrganizationAvatarDeleteCommand(organizationId)));



    #endregion
}