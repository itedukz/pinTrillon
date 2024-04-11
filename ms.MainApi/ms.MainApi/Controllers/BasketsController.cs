using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Business.Cqrs.Baskets;

namespace ms.MainApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BasketsController : BaseController
{
    #region DI
    private readonly IMediator _mediator;

    public BasketsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion


    /// <param name="productId"></param>
    /// <remarks>
    /// EndPoint для добавления продукта в корзину по productId:
    ///
    ///     POST api/Baskets/AddProduct/{1}
    ///
    /// </remarks>
    [HttpPost("AddProduct/{productId}")]
    public async Task<IActionResult> AddProduct([FromRoute] int productId) =>
        Return(await _mediator.Send(new BasketAddProductCommand(productId)));


    /// <param name="projectId"></param>
    /// <remarks>
    /// EndPoint для добавления проекта в корзину по projectId:
    ///
    ///     POST api/Baskets/AddProject/{1}
    ///
    /// </remarks>
    [HttpPost("AddProject/{projectId}")]
    public async Task<IActionResult> AddProject([FromRoute] int projectId) =>
        Return(await _mediator.Send(new BasketAddProjectCommand(projectId)));


    /// <remarks>
    /// EndPoint для получения списка продуктов в корзине:
    ///
    ///     GET /api/Baskets/ProductGetList
    ///     
    /// </remarks>
    [HttpGet("ProductGetList")]
    public async Task<IActionResult> ProductGetList() =>
        Return(await _mediator.Send(new BasketProductGetListCommand()));


    /// <remarks>
    /// EndPoint для получения списка проектов в корзине:
    ///
    ///     GET /api/Baskets/ProjectGetList
    ///     
    /// </remarks>
    [HttpGet("ProjectGetList")]
    public async Task<IActionResult> ProjectGetList() =>
        Return(await _mediator.Send(new BasketProjectGetListCommand()));


    /// <param name="productId"></param>
    /// <remarks>
    /// EndPoint для удаления продукта из корзины по productId:
    ///
    ///     POST api/Baskets/RemoveProduct/{1}
    ///
    /// </remarks>
    [HttpDelete("RemoveProduct/{productId}")]
    public async Task<IActionResult> RemoveProduct([FromRoute] int productId) =>
        Return(await _mediator.Send(new BasketRemoveProductCommand(productId)));


    /// <param name="projectId"></param>
    /// <remarks>
    /// EndPoint для удаления проекта из корзины по projectId:
    ///
    ///     POST api/Baskets/RemoveProject/{1}
    ///
    /// </remarks>
    [HttpDelete("RemoveProject/{projectId}")]
    public async Task<IActionResult> RemoveProject([FromRoute] int projectId) =>
        Return(await _mediator.Send(new BasketRemoveProjectCommand(projectId)));

}
