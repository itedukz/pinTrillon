using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Business.Cqrs.Favourites;
using ms.MainApi.Business.Cqrs.Measures;
using ms.MainApi.Entity.Models.DbModels.Projects;

namespace ms.MainApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FavouritesController : BaseController
{
    #region DI
    private readonly IMediator _mediator;

    public FavouritesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion


    /// <param name="productId"></param>
    /// <remarks>
    /// EndPoint для добавления продукта в избранное по productId:
    ///
    ///     POST api/Favourites/AddProduct/{1}
    ///
    /// </remarks>
    [HttpPost("AddProduct/{productId}")]
    public async Task<IActionResult> AddProduct([FromRoute] int productId) =>
        Return(await _mediator.Send(new FavouriteAddProductCommand(productId)));


    /// <param name="projectId"></param>
    /// <remarks>
    /// EndPoint для добавления проекта в избранное по projectId:
    ///
    ///     POST api/Favourites/AddProject/{1}
    ///
    /// </remarks>
    [HttpPost("AddProject/{projectId}")]
    public async Task<IActionResult> AddProject([FromRoute] int projectId) =>
        Return(await _mediator.Send(new FavouriteAddProjectCommand(projectId)));


    /// <remarks>
    /// EndPoint для получения списка избранных продуктов:
    ///
    ///     GET /api/Favourites/ProductGetList
    ///     
    /// </remarks>
    [HttpGet("ProductGetList")]
    public async Task<IActionResult> ProductGetList() =>
        Return(await _mediator.Send(new FavouriteProductGetListCommand()));


    /// <remarks>
    /// EndPoint для получения списка избранных проектов:
    ///
    ///     GET /api/Favourites/ProjectGetList
    ///     
    /// </remarks>
    [HttpGet("ProjectGetList")]
    public async Task<IActionResult> ProjectGetList() =>
        Return(await _mediator.Send(new FavouriteProjectGetListCommand()));


    /// <param name="productId"></param>
    /// <remarks>
    /// EndPoint для удаления продукта из избранных по productId:
    ///
    ///     POST api/Favourites/RemoveProduct/{1}
    ///
    /// </remarks>
    [HttpDelete("RemoveProduct/{productId}")]
    public async Task<IActionResult> RemoveProduct([FromRoute] int productId) =>
        Return(await _mediator.Send(new FavouriteRemoveProductCommand(productId)));


    /// <param name="projectId"></param>
    /// <remarks>
    /// EndPoint для удаления проекта из избранных по projectId:
    ///
    ///     POST api/Favourites/RemoveProject/{1}
    ///
    /// </remarks>
    [HttpDelete("RemoveProject/{projectId}")]
    public async Task<IActionResult> RemoveProject([FromRoute] int projectId) =>
        Return(await _mediator.Send(new FavouriteRemoveProjectCommand(projectId)));

}