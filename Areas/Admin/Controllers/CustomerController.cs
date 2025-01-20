using AspNetCoreHero.ToastNotification.Abstractions;
using Dash.Areas.Admin.Models;
using Dash.Areas.Identity.Models;
using Dash.Data;
using Dash.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dash.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]/[action]")]
public class CustomerController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ICustomerRepository _customerRepository;
    private readonly INotyfService _notyfService;

    public CustomerController(ApplicationDbContext context, ICustomerRepository customerRepository, INotyfService notyfService)
    {
        _context = context;
        _customerRepository = customerRepository;
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
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }
        
        var customers = await _customerRepository.GetAllCustomersAsync();
        ViewBag.user = ActiveUser;
        return View(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> CustomerDetails(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }
        var customer = await _customerRepository.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            _notyfService.Error("Customer not found.");
            return RedirectToAction("Index", "Customer", new {area = "Admin"});
        }
        ViewBag.user = ActiveUser;
        return View(customer);
    }

    [HttpGet]
    public async Task<IActionResult> AddCustomerPage()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }
        ViewBag.user = ActiveUser;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCustomer([FromForm] AddCustomerViewModel model)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        try
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Model is invalid.");
            }

            await _customerRepository.AddCustomerAsync(model);
            _notyfService.Success("Customer added.");
            ViewBag.user = ActiveUser;
            return RedirectToAction("Index", "Customer", new {area = "Admin"});
        }
        catch (Exception ex)
        {
            _notyfService.Error($"There was an error adding the customer. {ex.Message}");
            return RedirectToAction("Index", "Customer", new {area = "Admin"});
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> UpdateCustomerPage(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }
        var customer = await _customerRepository.GetCustomerByIdAsync(id);
        ViewBag.user = ActiveUser;
        return View(customer);
    }
    
    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateCustomer(int id, [FromForm] UpdateCustomerViewModel model)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        try
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Model is invalid.");
            }

            await _customerRepository.UpdateCustomerAsync(id, model);
            _notyfService.Success("Customer updated.");
            ViewBag.user = ActiveUser;
            return RedirectToAction("Index", "Customer", new {area = "Admin"});
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _notyfService.Error($"There has been a exception while updating the customer | DbUpdateConcurrencyException: {ex.Message}");
            return RedirectToAction("Index", "Customer", new { area = "Admin" });
        }
        catch (DbUpdateException ex)
        {
            _notyfService.Error($"There has been a exception while updating the customer | DbUpdateException: {ex.Message}");
            return RedirectToAction("Index", "Customer", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            _notyfService.Error($"There has been a exception while updating the customer | Exception: {ex.Message}");
            return RedirectToAction("Index", "Customer", new { area = "Admin" });
        }
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }
        await _customerRepository.DeleteCustomerAsync(id);
        _notyfService.Success("Customer deleted.");
        return RedirectToAction("Index", "Customer", new {area = "Admin"});
    }
}
