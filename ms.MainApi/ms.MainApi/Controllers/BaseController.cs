using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Entity.Models.Dtos.Responses;
using System.Net;

namespace ms.MainApi.Controllers;

public class BaseController : ControllerBase
{
    protected IActionResult Return(IMainResponseDto result)
    {
        return result.StatusCode switch
        {
            HttpStatusCode.BadRequest => BadRequest(result),
            HttpStatusCode.NotFound => NotFound(result),
            HttpStatusCode.Unauthorized => Unauthorized(result),
            HttpStatusCode.InternalServerError => BadRequest(result),
            HttpStatusCode.MethodNotAllowed => BadRequest(result),
            HttpStatusCode.Forbidden => Forbid(),
            _ => Ok(result)
        };
    }
}