using Dash.Areas.Identity.Models;
using Dash.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dash.Areas.Admin.Controllers;
[Area("Admin")]
[Route("[area]/[controller]/[action]")]
[Authorize]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }
    private AppUser? ActiveUser
    {
        get
        {
            return _context.AppUsers.FirstOrDefault(u => u.Id == HttpContext.Session.GetString("Id"));
        }
    }
    [HttpGet]
    public IActionResult Index()
    {
        if (ActiveUser == null)
        {
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        ViewBag.user = ActiveUser;
        return View();
    }
}