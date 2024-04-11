using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Business.Cqrs.Identities.Permissions;
using ms.MainApi.Entity.Models.Dtos.Identities.Permissions;
using ms.MainApi.Entity.Models.Dtos.Responses;
using System.Collections.Specialized;
using System.Net;

namespace ms.MainApi.Controllers.Identities;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PermissionsController : BaseController
{
    #region DI
    private readonly IMediator _mediator;

    public PermissionsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для связывания разрешении с ролью:
    ///
    ///      {
    ///        "roleId": 1,
    ///        "actions": [
    ///          1, 2, 3, 4     //1-getOwn, 2-getAll, 3-create, 4-update, 5-delete
    ///        ],
    ///        "permissionId": 1    //1-user, 2-role, 3-userRoleBind, 4-permission, 5-category, 6-product, 7-design, 8-project
    ///      }
    ///      
    /// </remarks>
    [HttpPost]
    [Route("bind")]
    public async Task<IActionResult> Bind([FromBody] PermissionCreateDto form) =>
        Return(await _mediator.Send(new PermissionCreateCommand(form)));

    /// <remarks>
    /// EndPoint для удаления разрешений с указанной роли:
    ///
    ///     DELETE api/Permissions/{1}/{1}
    ///
    /// </remarks>
    [HttpDelete]
    [Route("unbind/{roleId}/{permissionId}")]
    public async Task<IActionResult> unBind([FromRoute] int roleId, int permissionId) =>
        Return(await _mediator.Send(new PermissionDeleteCommand(roleId, permissionId)));

    /// <param name="id"></param>
    /// <remarks>
    /// EndPoint для получения роля по идентификатору разрешения:
    ///
    ///     GET api/Permissions/{1}
    ///
    /// </remarks>
    [HttpGet]
    [Route("{roleId}")]     //Рольмен Permission дарды сақтағаннан кейін қандай Action-дарға рұқсат берілгенін де қайтару керек.
    public async Task<IActionResult> GetByRoleId([FromRoute] int roleId) =>
        Return(await _mediator.Send(new PermissionGetByRoleCommand(roleId)));

    [HttpGet("getListByCurrentUser")]
    public async Task<IActionResult> GetByCurrentUser() =>
        Return(await _mediator.Send(new PermissionGetByCurrentUserCommand()));

    [HttpGet("getPermissionsDtlList")]
    public async Task<IActionResult> GetPermissionNameList() =>
        Return(await _mediator.Send(new PermissionGetNameListCommand()));


    [HttpGet("getCurrentUserPermission")]
    public async Task<IActionResult> GetCurrentUserPermission()
    {
        ListDictionary myCol = new ListDictionary();
        myCol.Add("canCreate", true);
        myCol.Add("canDelete", false);
        myCol.Add("canUpdate", true);
        myCol.Add("canGet", true);


        return Return(new MainResponseDto(myCol));
    }

}
