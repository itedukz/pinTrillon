using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Business.Cqrs.MainPages;
using ms.MainApi.Business.Cqrs.Measures;
using ms.MainApi.Entity.Models.Pages.SearchPages;

namespace ms.MainApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class MainPagesController : BaseController
{
    #region DI
    private readonly IMediator _mediator;

    public MainPagesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion


    /// <remarks>
    /// EndPoint для получения данных для главной станицы
    ///
    ///     GET /api/MainPages/catalogs
    ///     
    /// </remarks>
    [HttpGet("catalogs")]
    public async Task<IActionResult> GetCatalogs() =>
        Return(await _mediator.Send(new MainPageGetListCommand()));


    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для поиска товаров:
    ///
    ///     POST /api/MainPages/searchProduct
    ///     {
    ///       "query": {
    ///         "catalogsId": [
    ///           0
    ///         ],
    ///         "colorsId": [
    ///           0
    ///         ],
    ///         "materialsId": [
    ///           0
    ///         ],
    ///         "priceMin": 0,
    ///         "priceMax": 0,
    ///         "widthMin": 0,
    ///         "widthMax": 0,
    ///         "lengthMin": 0,
    ///         "lengthMax": 0,
    ///         "heightMin": 0,
    ///         "heightMax": 0
    ///       },
    ///       "sort": {
    ///         "orderByName": true,
    ///         "orderByPrice": true,
    ///         "orderByDate": true
    ///       },
    ///       "page": 0,
    ///       "pageSize": 0
    ///     }
    /// 
    /// </remarks>
    [HttpPost("searchProduct")]
    public async Task<IActionResult> SearchProduct([FromBody] SearchProduct form) =>
        Return(await _mediator.Send(new SearchPageProductGetListCommand(form)));


    /// <param name="form"></param>
    /// <remarks>
    /// EndPoint для поиска проектов:
    ///
    ///     POST /api/MainPages/searchProject
    ///     {
    ///       "query": {
    ///         "catalogsId": [
    ///           0
    ///         ],
    ///         "colorsId": [
    ///           0
    ///         ],
    ///         "priceMin": 0,
    ///         "priceMax": 0,
    ///         "quadratureMin": 0,
    ///         "quadratureMax": 0,
    ///         "widthMin": 0,
    ///         "widthMax": 0,
    ///         "lengthMin": 0,
    ///         "lengthMax": 0,
    ///         "heightMin": 0,
    ///         "heightMax": 0
    ///       },
    ///       "sort": {
    ///         "orderByName": true,
    ///         "orderByPrice": true,
    ///         "orderByDate": true
    ///       },
    ///       "page": 0,
    ///       "pageSize": 0
    ///     }
    /// 
    /// </remarks>
    [HttpPost("searchProject")]
    public async Task<IActionResult> SearchProject([FromBody] SearchProject form) =>
        Return(await _mediator.Send(new SearchPageProjectGetListCommand(form)));


    /// <remarks>
    /// EndPoint для получения списка значении сортировки:
    ///
    ///     POST /api/MainPages/sortOrderList
    ///     
    /// </remarks>
    [HttpGet("sortOrderList")]
    public async Task<IActionResult> SortOrderList() =>
        Return(await _mediator.Send(new SortEnumGetListCommand()));

}
