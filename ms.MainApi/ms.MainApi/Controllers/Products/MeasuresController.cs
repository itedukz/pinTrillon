using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Business.Cqrs.Measures;

namespace ms.MainApi.Controllers.Products;

[Route("api/[controller]")]
[ApiController]
public class MeasuresController : BaseController
{
    #region DI
    private readonly IMediator _mediator;

    public MeasuresController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion


    /// <remarks>
    /// EndPoint для получения списка единиц измерения габоритов товаров, проектов (mm, cm, m):
    ///
    ///     POST /api/Measures/list
    ///     
    /// </remarks>
    [HttpPost("list")]
    public async Task<IActionResult> GetList() =>
        Return(await _mediator.Send(new MeasureGetListEnumCommand()));

}
