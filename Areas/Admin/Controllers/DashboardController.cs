using AspNetCoreHero.ToastNotification.Abstractions;
using Dash.Areas.Admin.Models;
using Dash.Areas.Identity.Models;
using Dash.Data;
using Dash.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IJobRepository _jobRepository;
    private readonly ICampaignRepository _campaignRepository;
    private readonly ICampaignUserNoteRepository _campaignUserNoteRepository;
    private readonly ICampaignUserTaskRepository _campaignUserTaskRepository;
    private readonly INotyfService _notyfService;
    public DashboardController(ApplicationDbContext context, IUserRepository userRepository, IEventRepository eventRepository, 
        IProductRepository productRepository, ICustomerRepository customerRepository, INotyfService notyfService, 
        ICampaignRepository campaignRepository, ICampaignUserNoteRepository campaignUserNoteRepository,
        ICampaignUserTaskRepository campaignUserTaskRepository, IJobRepository jobRepository)
    {
        _context = context;
        _userRepository = userRepository;
        _eventRepository = eventRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _jobRepository = jobRepository;
        _campaignRepository = campaignRepository;
        _campaignUserNoteRepository = campaignUserNoteRepository;
        _campaignUserTaskRepository = campaignUserTaskRepository;
        _notyfService = notyfService;
    }
    private AppUser? ActiveUser
    {
        get
        {
            return _context.AppUsers.FirstOrDefault(u => u.Id == HttpContext.Session.GetString("Id"));
        }
    }

    private List<AppUser> GetUsers()
    {
        return _context.AppUsers.ToList();
    }
    private List<Event> GetEvents()
    {
        return _context.Events.ToList();
    }
    private List<Products> GetProducts()
    {
        return _context.Products.ToList();
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        var index = new IndexViewModel
        {
            UserEvents = await _eventRepository.GetAllUserEventsAsync(),
            Events = await _eventRepository.GetAllEventsAsync(),
            Products = await _productRepository.GetAllProductsAsync(),
            Users = await _userRepository.GetAllUsersAsync(),
            Jobs = await _jobRepository.GetAllJobsAsync(),
            Campaigns = await _campaignRepository.GetAllCampaignsAsync(),
            CampaignUserNotes = await _campaignUserNoteRepository.GetAllCampaignUserNotesAsync(),
            CampaignUserTasks = await _campaignUserTaskRepository.GetAllCampaignUserTasksAsync(),
            JobCount = await _jobRepository.CountJobsAsync(),
            UserCount = GetUsers().Count,
            EventCount = GetEvents().Count,
            ProductCount = GetProducts().Count,
            CustomerCount = await _customerRepository.CountCustomersAsync(),
            CampaignCount = await _campaignRepository.CampaignCountAsync(),
            CampaignUserNoteCount = await _campaignUserNoteRepository.CampaignUserNoteCountAsync(),
            CampaignUserTaskCount = await _campaignUserTaskRepository.CampaignUserTaskCountAsync(),
        };
        ViewBag.Users = await _userRepository.GetAllUsersAsync();
        ViewBag.Events = await _eventRepository.GetAllEventsAsync();
        ViewBag.Products = await _productRepository.GetAllProductsAsync();
        ViewBag.user = ActiveUser;
        return View(index);
    }

  
   

}