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
public class JobController : Controller
{
    private readonly  ApplicationDbContext _context;
    private readonly IJobRepository _jobRepository;
    private readonly INotyfService _notyfService;
    public JobController(ApplicationDbContext context, IJobRepository jobRepository, INotyfService notyfService)
    {
        _context = context;
        _jobRepository = jobRepository;
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
            _notyfService.Error("You are not logged in, you need to be to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        var jobs = await _jobRepository.GetAllJobsAsync();
        ViewBag.user = ActiveUser;
        return View(jobs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> JobDetails(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, you need to be to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }
        
        var job = await _jobRepository.GetJobByIdAsync(id);
        ViewBag.user = ActiveUser;
        return View(job);
    }

    [HttpGet]
    public async Task<IActionResult> AddJobPage()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, you need to be to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }
        ViewBag.users = await _context.Users.ToListAsync();
        ViewBag.user = ActiveUser;
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddJob([FromForm] AddJobsViewModel model)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, you need to be to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        try
        {
            await _jobRepository.AddJobAsync(model);
            _notyfService.Success("Job added.");
            ViewBag.user = ActiveUser;
            return RedirectToAction("Index", "Job", new {area = "Admin"});
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return RedirectToAction("Index", "Job", new { area = "Admin" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> UpdateJobPage(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, you need to be to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }
        var job = await _jobRepository.GetJobByIdAsync(id);
        ViewBag.user = ActiveUser;
        ViewBag.users = await _context.Users.ToListAsync();
        return View(job);
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateJob(int id, [FromForm] UpdateJobsViewModel model)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, you need to be to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        try
        {
            await _jobRepository.UpdateJobAsync(id, model);
            _notyfService.Success("Job updated.");
            ViewBag.users = await _context.Users.ToListAsync();
            ViewBag.user = ActiveUser;
            return RedirectToAction("Index", "Job", new {area = "Admin"});
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _notyfService.Error(ex.Message);
            return RedirectToAction("Index", "Job", new { area = "Admin" });
        }
        catch (DbUpdateException ex)
        {
            _notyfService.Error(ex.Message);
            return RedirectToAction("Index", "Job", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return RedirectToAction("Index", "Job", new { area = "Admin" });
        }
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteJob(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, you need to be to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }
        await _jobRepository.DeleteJobAsync(id);
        _notyfService.Success("Job deleted.");
        ViewBag.user = ActiveUser;
        ViewBag.users = await _context.Users.ToListAsync();
        return RedirectToAction("Index", "Job", new { area = "Admin" });
    }
}