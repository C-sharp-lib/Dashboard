using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Dash.Areas.Admin.Models;
using Dash.Areas.Identity.Models;
using Dash.Data;
using Dash.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dash.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]/[action]")]
[Authorize]
public class EventController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IEventRepository _eventRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly INotyfService _notyfService;
    public EventController(ApplicationDbContext context, IEventRepository eventRepository, IUserRepository userRepository, 
        IProductRepository productRepository, INotyfService notyfService)
    {
        _context = context;
        _eventRepository = eventRepository;
        _userRepository = userRepository;
        _productRepository = productRepository;
        _notyfService = notyfService;
    }
    private AppUser? ActiveUser
    {
        get
        {
            return _context.AppUsers.FirstOrDefault(u => u.Id == HttpContext.Session.GetString("Id"));
        }
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to log in to view this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }

        var events = await _eventRepository.GetAllEventsAsync();
        ViewBag.user = ActiveUser;
        return View(events);
    }
    [HttpGet]
    public async Task<IActionResult> Calendar()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }

        var index = new IndexViewModel
        {
            UserEvents = await _context.UserEvents.ToListAsync(),
            Products = await _context.Products.ToListAsync(),
            Events = await _context.Events.ToListAsync(),
            Users = await _context.Users.ToListAsync(),
            UserCount = await _userRepository.CountUsersAsync(),
            EventCount = await _eventRepository.EventCountAsync(),
            ProductCount = await _productRepository.CountProductsAsync()
        };
        ViewBag.user = ActiveUser;
        return View(index);
    }

     [HttpGet("{id}")]
    public async Task<IActionResult> EventDetails(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        var events = await _eventRepository.GetEventByIdAsync(id);
        ViewBag.user = ActiveUser;
        return View(events);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddEvent([FromForm] EventViewModel events)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        try
        {
            if (ModelState.IsValid)
            {
                await _eventRepository.AddEventAsync(events);
                _notyfService.Success("Event Added Successfully");
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }
        catch (Exception ex)
        {
            _notyfService.Error($"There has been an Exception: {ex.Message}");
            return RedirectToAction("AddEventPage", "Event", new { area = "Admin" });
        }
        ViewBag.user = ActiveUser;
        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateEvents([FromForm] UpdateEventViewModel events)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        try
        {
            await _eventRepository.UpdateEventAsync(events);
            _notyfService.Success("Event Updated Successfully");
            ViewBag.user = ActiveUser;
            return RedirectToAction("Calendar", "Event", new { area = "Admin" });
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _notyfService.Error($"There has been a exception while updating the event | DbUpdateConcurrencyException: {ex.Message}");
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
        catch (DbUpdateException ex)
        {
            _notyfService.Error($"There has been a exception while updating the event | DbUpdateException: {ex.Message}");
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            _notyfService.Error($"There has been a exception while updating the event | Exception: {ex.Message}");
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
    }

    
    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateEventsById(int id, [FromForm] UpdateEventViewModel events)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        try
        {
            await _eventRepository.UpdateEventByIdAsync(id, events);
            _notyfService.Success("Event Updated Successfully");
            ViewBag.user = ActiveUser;
            return RedirectToAction("Calendar", "Event", new { area = "Admin" });
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _notyfService.Error($"There has been a exception while updating the event | DbUpdateConcurrencyException: {ex.Message}");
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
        catch (DbUpdateException ex)
        {
            _notyfService.Error($"There has been a exception while updating the event | DbUpdateException: {ex.Message}");
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            _notyfService.Error($"There has been a exception while updating the event | Exception: {ex.Message}");
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
    }
    
    
    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteEvents(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        await _eventRepository.DeleteEventAsync(id);
        _notyfService.Success("Event deleted successfully");
        ViewBag.user = ActiveUser;
        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> UpdateEventPage(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }

        var eve = await _eventRepository.GetEventByIdAsync(id);
        ViewBag.user = ActiveUser;
        ViewBag.Event = eve;
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AddEventPage()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        ViewBag.user = ActiveUser;
        return View();
    }
}