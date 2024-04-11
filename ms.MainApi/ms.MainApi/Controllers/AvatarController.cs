using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms.MainApi.Business.Cqrs.Products.ProductPictures;

namespace ms.MainApi.Controllers;

//[Authorize]
public class AvatarController : Controller
{
    #region DI
    private readonly IMediator _mediator;

    public AvatarController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion


    public IActionResult Index()
    {
        return View();
    }

    //[Authorize]
    public async Task<IActionResult> Create(int productId, IFormFile? avatar)
    {

        Ok(await _mediator.Send(new ProductPictureCreateCommand(productId, avatar, false)));

        return RedirectToAction("Index");
    }
        
}
