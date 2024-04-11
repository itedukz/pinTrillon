using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Business.Cqrs.Identities.Users;
using ms.MainApi.DataAccess;
using ms.MainApi.Entity.Models.Dtos.Identities.Users;
using ms.MainApi.Entity.Models.Services;

namespace ms.MainApi.Controllers.Identities;

[Route("api/[controller]")]
[ApiController]
public class UsersController : BaseController
{
    #region DI
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion


    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для получения пользователя по идентификатору:
    ///
    ///     GET api/Users/{1}
    ///
    /// </remarks>
    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    public async Task<IActionResult> GetBy([FromRoute] int id) =>
        Return(await _mediator.Send(new UserGetCommand(id)));

    [Authorize]
    [HttpGet("getCurrentUser")]
    public async Task<IActionResult> GetCurrentUser() =>
        Return(await _mediator.Send(new UserGetCurrentCommand()));


    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для динамической фильтрации запросов:
    ///
    ///     POST /api/Users/list
    ///     {    
    ///       "search": "Алмат",    //return record where [firstName, lastName, middleName, email, phoneNumber] Contains "Алмат" with toLower
    ///       "query": {
    ///         "condition": "and",
    ///         "rules": [
    ///           {
    ///             "field": "firstName",
    ///             "operators": "equal",
    ///             "type": "string",
    ///             "value": "Алмат",
    ///             "values": [
    ///               "string"
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
    [Authorize]
    [HttpPost("list")]
    public async Task<IActionResult> GetList([FromBody] QueryPagination form) =>
        Return(await _mediator.Send(new UserGetListCommand(form)));


    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для создания нового пользователя:
    ///
    ///     POST /api/Users
    ///     {
    ///        "firstName": "Алмат",
    ///        "lastName": "Аженов",
    ///        "middleName": "",
    ///        "phoneNumber": "+77472203400",
    ///        "email": "azhenov@mail.ru",
    ///        "password": "string"
    ///     }
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateDto form) =>
        Return(await _mediator.Send(new UserCreateCommand(form)));


    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для редактирования пользователя по Id:
    ///
    ///     PUT /api/Users
    ///     {
    ///        "id": 1
    ///        "firstName": "Almat",
    ///        "lastName": "Azhenov",
    ///        "middleName": "",
    ///        "phoneNumber": "+77472203400"
    ///     }
    /// </remarks>
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserUpdateDto form) =>
        Return(await _mediator.Send(new UserUpdateCommand(form)));


    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для заблокирования пользователя по userId:
    ///
    ///     PUT /api/Users/Bun
    ///     {
    ///        "userId": 1
    ///        "isBan": true    #Забанить пользователя с ID = 1. Если "isBan": false то разблокировать пользователя с ID = 1
    ///     }
    /// </remarks>
    [Authorize]
    [HttpPut("Ban")]
    public async Task<IActionResult> Bun([FromBody] UserBanDto form) =>
        Return(await _mediator.Send(new UserBanCommand(form)));


    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для удаления пользователя по идентификатору:
    ///
    ///     DELETE api/Users/{1}
    ///
    /// </remarks>
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id) =>
        Return(await _mediator.Send(new UserDeleteCommand(id)));


    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для изменения паролья для текущего пользователя:
    ///
    ///     POST /api/Users/changePassword
    ///     {
    ///        "password": "123456"
    ///     }
    /// </remarks>
    [Authorize]
    [HttpPost]
    [Route("changePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] UserUpdatePasswordDto form) =>
        Return(await _mediator.Send(new UserUpdatePasswordCommand(form)));


    /// <param name="userId"></param>
    /// <remarks>
    /// EndPoint для сбрасывания пароля по userId:
    ///
    ///     DELETE api/Users/resetPassword/{1}
    ///     
    /// </remarks>
    [Authorize]
    [HttpGet("resetPassword/{userId}")]
    public async Task<IActionResult> ResetPassword([FromRoute] int userId) =>
        Return(await _mediator.Send(new UserResetPasswordCommand(userId)));


    /// <param name="userId"></param>
    /// <param name="avatar"></param>
    /// <remarks>
    /// EndPoint для загрузки аватара пользователя. Отправить через [FromForm] IFormFile? avatar
    ///
    ///     POST /api/Users/avatar/{1}
    ///     {
    ///        "avatar": "input Type = 'file', name = 'avatar'"
    ///     }
    /// </remarks>
    [Authorize]
    [RequestSizeLimit(1024 * 1024 * 5)]
    [HttpPost("avatar/{userId}")]
    public async Task<IActionResult> Upload([FromRoute] int userId, [FromForm] IFormFile? avatar) =>
        Return(await _mediator.Send(new UserAvatarUploadCommand(userId, avatar)));


    ///// <param name="userId"></param>
    ///// <param name="avatar"></param>
    ///// <remarks>
    ///// EndPoint для загрузки аватара пользователя. Отправить через [FromForm] IFormFile? avatar
    /////
    /////     POST /api/Users/{1}/upload/avatar
    /////     {
    /////        "avatar": "input Type = 'file', name = 'avatar'"
    /////     }
    ///// </remarks>
    //[HttpPost("{userId}/upload/avatar")]
    //public async Task<IActionResult> UploadWithoutBoby([FromRoute] int userId, IFormFile? avatar) =>
    //    Return(await _mediator.Send(new UserUploadAvatarCommand(userId, avatar)));

    ///// <param name="userId"></param>
    ///// <param name="avatar"></param>
    ///// <remarks>
    ///// EndPoint для загрузки аватара пользователя. Отправить через [FromForm] IFormFile? avatar
    /////
    /////     POST /api/Users/{1}/UploadFromBoby/avatar
    /////     {
    /////        "avatar": "input Type = 'file', name = 'avatar'"
    /////     }
    ///// </remarks>
    //[HttpPost("{userId}/UploadFromBody/avatar")]
    //public async Task<IActionResult> UploadFromBody([FromRoute] int userId, [FromBody] IFormFile? avatar) =>
    //    Return(await _mediator.Send(new UserUploadAvatarCommand(userId, avatar)));

}