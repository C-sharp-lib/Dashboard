using Microsoft.AspNetCore.Mvc;

namespace Dash.Controllers;

[Area("Admin")]
[Route("[area]/[controller]/[action]")]
public class ErrorController : Controller
{
    public ErrorController()
    {
        
    }

    [HttpGet]
    public IActionResult StatusCode400()
    {
        return View();
    }
    [HttpGet]
    public IActionResult StatusCode404()
    {
        return View();
    }
    [HttpGet]
    public IActionResult StatusCode500()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Generic()
    {
        return View();
    }

    [Route("{statusCode}")]
    public IActionResult HttpStatusCodeHandler(int statusCode)
    {
        switch (statusCode)
        {
            case 400:
                return View(nameof(StatusCode400));
            case 404:
                return View(nameof(StatusCode404));
            case 500:
                return View(nameof(StatusCode500));
            default:
                return View(nameof(Generic));
        }
    }

    [Route("Exception")]
    public IActionResult ExceptionHandler()
    {
        return View("Generic");
    }
}