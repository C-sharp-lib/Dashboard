using Dash.Areas.Admin.Models;
using Dash.Areas.Identity.Models;
using Dash.Data;
using Dash.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Exception = System.Exception;

namespace Dash.Areas.Admin.Controllers;
[Area("Admin")]
[Route("[area]/[controller]/[action]")]
[Authorize]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IEventRepository _eventRepository;
    public DashboardController(ApplicationDbContext context, IUserRepository userRepository, IEventRepository eventRepository)
    {
        _context = context;
        _userRepository = userRepository;
        _eventRepository = eventRepository;
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
            ModelState.AddModelError(string.Empty, "You are not logged in");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }

        var index = new IndexViewModel
        {
            UserEvents = await _eventRepository.GetAllUserEventsAsync(),
            Events = await _eventRepository.GetAllEventsAsync(),
            Users = await _userRepository.GetAllUsersAsync(),
            UserCount = await _userRepository.CountUsersAsync(),
            EventCount = await _eventRepository.EventCountAsync()
        };
        ViewBag.user = ActiveUser;
        return View(index);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> EventDetails(int id)
    {
        if (ActiveUser == null)
        {
            ModelState.AddModelError(string.Empty, "You are not logged in");
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
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        try
        {
            if (ModelState.IsValid)
            {
                await _eventRepository.AddEventAsync(events);
                ViewBag.SuccessMessage = "Event created successfully";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"There has been an exception adding the event: {ex.Message}");
            return RedirectToAction("AddEventPage", "Dashboard", new { area = "Admin" });
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
            ModelState.AddModelError("", "You are not logged in");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        try
        {
            await _eventRepository.UpdateEventAsync(events);
            ViewBag.SuccessMessage = "Event updated successfully";
            ViewBag.user = ActiveUser;
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
        catch (DbUpdateConcurrencyException ex)
        {
            ViewBag.ErrorMessage = $"There was an exception updating the events: {ex.Message}";
            ModelState.AddModelError("", $"There has been a DbUpdateConcurrencyException: {ex.Message}");
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
        catch (DbUpdateException ex)
        {
            ViewBag.ErrorMessage = $"There was an exception updating the events: {ex.Message}";
            ModelState.AddModelError("", $"There has been an DBUpdateException: {ex.Message}");
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"There was an exception updating the events: {ex.Message}";
            ModelState.AddModelError("", $"There has been an exception: {ex.Message}");
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
    }

    [HttpDelete("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteEvents(int id)
    {
        if (ActiveUser == null)
        {
            ModelState.AddModelError("", "You are not logged in");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        await _eventRepository.DeleteEventAsync(id);
        ViewBag.SuccessMessage = "Event deleted successfully";
        ViewBag.user = ActiveUser;
        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> UpdateEvent(int id)
    {
        if (ActiveUser == null)
        {
            ModelState.AddModelError("", "You are not logged in");
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
            ModelState.AddModelError("", "You are not logged in");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        ViewBag.user = ActiveUser;
        return View();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> UserEventDetails(int id)
    {
        if (ActiveUser == null)
        {
            ModelState.AddModelError("", "You are not logged in");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        var eu = await _eventRepository.GetEventByIdAsync(id);
        ViewBag.user = ActiveUser;
        return View(eu);
    }

    [HttpDelete("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUserEvent(int id)
    {
        if (ActiveUser == null)
        {
            ModelState.AddModelError("", "You are not logged in");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        await _eventRepository.DeleteEventAsync(id);
        ViewBag.SuccessMessage = "Event deleted successfully";
        ViewBag.user = ActiveUser;
        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
    }
}