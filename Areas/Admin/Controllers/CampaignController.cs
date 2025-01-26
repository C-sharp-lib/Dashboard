using AspNetCoreHero.ToastNotification.Abstractions;
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
public class CampaignController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ICampaignRepository _campaignRepository;
    private readonly ICampaignUserNoteRepository _campaignUserNoteRepository;
    private readonly ICampaignUserTaskRepository _campaignUserTaskRepository;
    private readonly INotyfService _notyfService;
    public CampaignController(ApplicationDbContext context, ICampaignRepository campaignRepository, 
        ICampaignUserNoteRepository campaignUserNoteRepository,
        ICampaignUserTaskRepository campaignUserTaskRepository, INotyfService notyfService)
    {
        _notyfService = notyfService;
        _context = context;
        _campaignRepository = campaignRepository;
        _campaignUserNoteRepository = campaignUserNoteRepository;
        _campaignUserTaskRepository = campaignUserTaskRepository;
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
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }
        var campaigns = await _campaignRepository.GetAllCampaignsAsync();
        ViewBag.user = ActiveUser;
        return View(campaigns);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> CampaignDetails(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }
        var campaign = await _campaignRepository.GetCampaignByIdAsync(id);
        if (campaign == null)
        {
            _notyfService.Error("Campaign not found.");
            return RedirectToAction("Index", "Campaign", new {area = "Admin"});
        }
        ViewBag.user = ActiveUser;
        return View(campaign);
    }
    
    [HttpGet]
    public async Task<IActionResult> AddCampaignPage()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }
        ViewBag.user = ActiveUser;
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AddCampaignUserNotePage()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        ViewBag.campaigns = await _context.Campaigns.ToListAsync();
        ViewBag.users = await _context.Users.ToListAsync();
        ViewBag.user = ActiveUser;
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> AddCampaignUserTaskPage()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        ViewBag.campaigns = await _context.Campaigns.ToListAsync();
        ViewBag.users = await _context.Users.ToListAsync();
        ViewBag.user = ActiveUser;
        return View();
    }
        
    [HttpGet("{id}")]
    public async Task<IActionResult> UpdateCampaignPage(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        var campaign = await _context.Campaigns
            .Include(c => c.CampaignUserNotes)
            .ThenInclude(c => c.User)
            .Include(c => c.CampaignUserTasks)
            .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(c => c.CampaignId == id);
        if (campaign == null)
        {
            _notyfService.Error("Campaign not found.");
            return RedirectToAction("Index", "Campaign", new {area = "Admin"});
        }
        ViewBag.user = ActiveUser;
        return View(campaign);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> UpdateCampaignTasksPage(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        var campaign = await _context.CampaignUserTasks
            .Include(c => c.Campaign)
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.CampaignUserTaskId == id);
        if (campaign == null)
        {
            _notyfService.Error("Campaign user task not found.");
            return RedirectToAction("Index", "Campaign", new {area = "Admin"});
        }
        ViewBag.users = await _context.Users.ToListAsync();
        ViewBag.campaigns = await _context.Campaigns.ToListAsync();
        ViewBag.user = ActiveUser;
        return View(campaign);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> UpdateCampaignNotesPage(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        var campaign = await _context.CampaignUserNotes
            .Include(c => c.Campaign)
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.CampaignUserNoteId == id);
        if (campaign == null)
        {
            _notyfService.Error("Campaign user note not found.");
            return RedirectToAction("Index", "Campaign", new {area = "Admin"});
        }
        ViewBag.users = await _context.Users.ToListAsync();
        ViewBag.campaigns = await _context.Campaigns.ToListAsync();
        ViewBag.user = ActiveUser;
        return View(campaign);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> CampaignUserNoteDetails(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        var note = await _context.CampaignUserNotes
            .Include(c => c.Campaign)
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.CampaignUserNoteId == id);
        if (note == null)
        {
            _notyfService.Error("Campaign user note not found.");
            return RedirectToAction("Index", "Campaign", new {area = "Admin"});
        }
        ViewBag.user = ActiveUser;
        return View(note);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> CampaignUserTaskDetails(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        var task = await _context.CampaignUserTasks
            .Include(c => c.Campaign)
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.CampaignUserTaskId == id);
        if (task == null)
        {
            _notyfService.Error("Campaign user note not found.");
            return RedirectToAction("Index", "Campaign", new {area = "Admin"});
        }
        ViewBag.user = ActiveUser;
        return View(task);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCampaign([FromForm] AddCampaignViewModel model)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        try
        {
            await _campaignRepository.AddCampaignAsync(model);
            _notyfService.Success("Campaign added.");
            ViewBag.user = ActiveUser;
            return RedirectToAction("Index", "Campaign", new {area = "Admin"});
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return RedirectToAction("Index", "Campaign", new {area = "Admin"});
        }
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateCampaign(int id, [FromForm] UpdateCampaignViewModel model)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        try
        {
            await _campaignRepository.UpdateCampaignAsync(id, model);
            _notyfService.Success("Campaign updated.");
            ViewBag.user = ActiveUser;
            return RedirectToAction("Index", "Campaign", new {area = "Admin"});
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _notyfService.Error($"There has been a exception while updating the campaign | DbUpdateConcurrencyException: {ex.Message}");
            return RedirectToAction("Index", "Campaign", new { area = "Admin" });
        }
        catch (DbUpdateException ex)
        {
            _notyfService.Error($"There has been a exception while updating the campaign | DbUpdateException: {ex.Message}");
            return RedirectToAction("Index", "Campaign", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            _notyfService.Error($"There has been a exception while updating the campaign | Exception: {ex.Message}");
            return RedirectToAction("Index", "Campaign", new { area = "Admin" });
        }
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCampaign(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }
        var campaign = await _campaignRepository.GetCampaignByIdAsync(id);
        if (campaign == null)
        {
            _notyfService.Error("Campaign not found");
            return RedirectToAction("Index", "Campaign", new {area = "Admin"});
        }
        await _campaignRepository.DeleteCampaignAsync(id);
        ViewBag.user = ActiveUser;
        return RedirectToAction("Index", "Campaign", new { area = "Admin" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCampaignUserNote([FromForm] AddCampaignUserNotesViewModel model)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        try
        {
            ViewBag.campaigns = await _campaignRepository.GetAllCampaignsAsync();
            ViewBag.users = await _context.Users.ToListAsync();
            await _campaignUserNoteRepository.AddCampaignUserNoteAsync(model);
            _notyfService.Success("Campaign User Note Added.");
            ViewBag.user = ActiveUser;
            return RedirectToAction("Index", "Campaign", new {area = "Admin"});
        }
        catch (Exception ex)
        {
            _notyfService.Error($"There has been a exception while updating the campaign | Exception: {ex.Message}");
            return RedirectToAction("Index", "Campaign", new { area = "Admin" });
        }
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateCampaignUserNote(int id, [FromForm] UpdateCampaignUserNotesViewModel model)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        try
        {
            await _campaignUserNoteRepository.UpdateCampaignUserNoteAsync(id, model);
            _notyfService.Success("Campaign User Note Updated.");
            ViewBag.user = ActiveUser;
            return RedirectToAction("Index", "Campaign", new {area = "Admin"});
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _notyfService.Error($"There has been a exception while updating the campaign user note | DbUpdateConcurrencyException: {ex.Message}");
            return RedirectToAction("Index", "Campaign", new { area = "Admin" });
        }
        catch (DbUpdateException ex)
        {
            _notyfService.Error($"There has been a exception while updating the campaign user note | DbUpdateException: {ex.Message}");
            return RedirectToAction("Index", "Campaign", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            _notyfService.Error($"There has been a exception while updating the campaign user note | Exception: {ex.Message}");
            return RedirectToAction("Index", "Campaign", new { area = "Admin" });
        }
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCampaignUserNote(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }
        await _campaignUserNoteRepository.DeleteCampaignUserNoteAsync(id);
        _notyfService.Success("Campaign User Note Deleted.");
        ViewBag.user = ActiveUser;
        return RedirectToAction("Index", "Campaign", new {area = "Admin"});
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCampaignUserTasks([FromForm] AddCampaignUserTasksViewModel model)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        try
        {
            await _campaignUserTaskRepository.AddCampaignUserTaskAsync(model);
            _notyfService.Success("Campaign User Tasks Added.");
            ViewBag.user = ActiveUser;
            return RedirectToAction("Index", "Campaign", new {area = "Admin"});
        }
        catch (Exception ex)
        {
            _notyfService.Error($"There has been an Exception adding the campaign tasks | Exception: {ex.Message}");
            return RedirectToAction("Index", "Campaign", new {area = "Admin"});
        }
    }


    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateCampaignUserTasks(int id, [FromForm] UpdateCampaignUserTasksViewModel model)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        try
        {
            await _campaignUserTaskRepository.UpdateCampaignUserTaskAsync(id, model);
            _notyfService.Success("Campaign User Tasks Updated.");
            ViewBag.user = ActiveUser;
            return RedirectToAction("Index", "Campaign", new {area = "Admin"});
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _notyfService.Error($"There has been a exception while updating the campaign user task | DbUpdateConcurrencyException: {ex.Message}");
            return RedirectToAction("Index", "Campaign", new { area = "Admin" });
        }
        catch (DbUpdateException ex)
        {
            _notyfService.Error($"There has been a exception while updating the campaign user task | DbUpdateException: {ex.Message}");
            return RedirectToAction("Index", "Campaign", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            _notyfService.Error($"There has been a exception while updating the campaign user task | Exception: {ex.Message}");
            return RedirectToAction("Index", "Campaign", new { area = "Admin" });
        }
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCampaignUserTasks(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }
        await _campaignUserTaskRepository.DeleteCampaignUserTaskAsync(id);
        _notyfService.Success("Campaign User Tasks Deleted.");
        ViewBag.user = ActiveUser;
        return RedirectToAction("Index", "Campaign", new {area = "Admin"});
    }
}
















