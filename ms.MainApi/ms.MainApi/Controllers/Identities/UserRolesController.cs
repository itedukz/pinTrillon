using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Business.Cqrs.Identities.UserRoles;
using ms.MainApi.Entity.Models.Dtos.Identities.UserRoles;

namespace ms.MainApi.Controllers.Identities;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserRolesController : BaseController
{
    #region DI
    private readonly IMediator _mediator;

    public UserRolesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для связывания списка ролей с указанным пользователем по userId:
    ///
    ///     {
    ///        "userId": 1,
    ///        "rolesId": [
    ///            1, 3, 4      //пользователем с userId=1 будет связан с ролями 1,3,4
    ///          ]
    ///     }
    /// </remarks>
    [HttpPost]
    [Route("bind")]
    public async Task<IActionResult> Bind([FromBody] UserRoleCreateDto form) =>
        Return(await _mediator.Send(new UserRoleCreateCommand(form)));

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для редактирования роля по Id:
    ///
    ///     {
    ///        "userId": 1,
    ///        "rolesId": [
    ///           1, 3, 4           //пользователем с userId=1 будет связан с ролями 1,3,4
    ///        ]
    ///     }
    /// </remarks>
    [HttpPut]
    [Route("unbind")]
    public async Task<IActionResult> unBind([FromBody] UserRoleCreateDto form) =>
        Return(await _mediator.Send(new UserRoleDeleteCommand(form)));

    /// <param name="roleId"></param>
    /// <remarks>
    /// EndPoint для получения Пользователей по идентификатору роля:
    ///
    ///     GET api/UserRoles/Users/{1}
    ///
    /// </remarks>
    [HttpGet]
    [Route("Users/{roleId}")]
    public async Task<IActionResult> UsersByRole([FromRoute] int roleId) =>
        Return(await _mediator.Send(new UserRoleGetUsersCommand(roleId)));

    /// <param name="userId"></param>
    /// <remarks>
    /// EndPoint для получения Ролей по идентификатору пользователя:
    ///
    ///     GET api/UserRoles/Roles/{1}
    ///
    /// </remarks>
    [HttpGet]
    [Route("Roles/{userId}")]
    public async Task<IActionResult> RolesByUser([FromRoute] int userId) =>
        Return(await _mediator.Send(new UserRoleGetRolesCommand(userId)));

}