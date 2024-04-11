using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Business.Cqrs.Identities.Users;

namespace ms.MainApi.Controllers.Identities;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class SocialOAuthController : ControllerBase
{
    #region DI
    private readonly IMediator _mediator;

    public SocialOAuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    [HttpGet("RedirectToGoogle")]
    public async Task<IActionResult> RedirectToGoogle()
    {
        var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };

        //var result = Challenge(properties, GoogleDefaults.AuthenticationScheme);
        await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
        {
            RedirectUri = Url.Action("GoogleResponse")
        });

        return Ok();  //result
    }
    

    
    [HttpGet("google-response")]
    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
        {
            claim.Issuer,
            claim.OriginalIssuer,
            claim.Type,
            claim.Value
        });

        
        return Ok(claims);
    }
    



}
