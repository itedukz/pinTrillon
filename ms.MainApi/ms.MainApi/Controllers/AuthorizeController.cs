using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Business.Cqrs.Identities;
using ms.MainApi.Entity.Models.Dtos.Identities.Users;

namespace ms.MainApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorizeController : BaseController
{
    #region DI
    private readonly IMediator _mediator;

    public AuthorizeController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для авторизаци:
    ///
    ///     POST /api/Authorize
    ///     {
    ///        "email": "itedu.kz@gmail.com",
    ///        "password": "string"
    ///     }
    ///
    /// </remarks>
    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto form) =>
        Return(await _mediator.Send(new AuthorizeLoginCommand(form)));


    [HttpGet]
    [AllowAnonymous]
    [Route("logout")]
    public async Task<IActionResult> Logout() =>
        Return(await _mediator.Send(new AuthorizeLogoutCommand()));

}
