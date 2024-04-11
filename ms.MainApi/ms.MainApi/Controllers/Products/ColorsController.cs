using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Business.Cqrs.Products.Colors;
using ms.MainApi.Entity.Models.Services;

namespace ms.MainApi.Controllers.Products;

[Route("api/[controller]")]
[ApiController]
public class ColorsController : BaseController
{
    #region DI
    private readonly IMediator _mediator;

    public ColorsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion


    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для динамической фильтрации запросов:
    ///
    ///     POST /api/Colors/list
    ///     {    
    ///       "search": "White",    //return record where [nameEng, nameRus] Contains "white" with lower case
    ///       "query": {
    ///         "condition": "and",
    ///         "rules": [
    ///           {
    ///             "field": "nameRus",
    ///             "operators": "equal",
    ///             "type": "string",
    ///             "value": "White",       //return record where nameRus == "White"
    ///             "values": [
    ///               "", ""    
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
        Return(await _mediator.Send(new ColorGetListEnumCommand(form)));

}
