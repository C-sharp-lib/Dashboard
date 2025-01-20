using AspNetCoreHero.ToastNotification.Abstractions;
using Dash.Areas.Identity.Models;
using Dash.Data;
using Dash.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dash.Areas.Identity.Controllers;

[Area("Identity")]
[Route("[area]/[controller]/[action]")]
public class IdentityController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webenv;
    private readonly ApplicationDbContext _context;
    private readonly IUserRepository _userRepository;
    private readonly INotyfService _notyfService;
    public IdentityController(UserManager<AppUser> userM, SignInManager<AppUser> signInM, 
        IHttpContextAccessor contextAccessor, ApplicationDbContext context, IWebHostEnvironment webenv,
        IUserRepository userRepository, INotyfService notyfService)
    {
        _userManager = userM;
        _signInManager = signInM;
        _httpContextAccessor = contextAccessor;
        _context = context;
        _webenv = webenv;
        _userRepository = userRepository;
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
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in. You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        var users = await _userRepository.GetAllUsersAsync();
        ViewBag.user = ActiveUser;
        return View(users);
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    HttpContext.Session.SetString("Id", user.Id);
                    HttpContext.Session.SetString("UserName", user.UserName);
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                _notyfService.Error("Invalid login attempt.");
            }
            else
            {
                _notyfService.Error("User does not exist.");
            }
        }
        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            string uniqueFileName3 = null;
            if (model.ImageUrl != null && model.ImageUrl.Length > 0)
            {
                var permittedExtensions3 = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension3 = Path.GetExtension(model.ImageUrl.FileName).ToLowerInvariant();

                if (string.IsNullOrEmpty(extension3) || !permittedExtensions3.Contains(extension3))
                {
                    _notyfService.Error("Invalid image extension.");
                }
                string fileName3 = Path.GetFileNameWithoutExtension(model.ImageUrl.FileName);
                uniqueFileName3 = $"{fileName3}_{Guid.NewGuid()}{extension3}";
                string uploadsFolder3 = Path.Combine(_webenv.WebRootPath, "Uploads/AppUsers/");
                if (!Directory.Exists(uploadsFolder3))
                {
                    Directory.CreateDirectory(uploadsFolder3);
                }
                string filePath = Path.Combine(uploadsFolder3, uniqueFileName3);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageUrl.CopyToAsync(fileStream);
                }
            }
            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                DateOfHire = model.DateOfHire,
                Address = model.Address,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                ImageUrl = (uniqueFileName3 != null ? Path.Combine("Uploads/AppUsers/", uniqueFileName3) : null)!
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                HttpContext.Session.SetString("Id", user.Id);
                HttpContext.Session.SetString("UserName", user.UserName);
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        ViewBag.user = ActiveUser;
        await _signInManager.SignOutAsync();
        _notyfService.Information("User logged out.");
        return RedirectToAction("Login", "Identity", new { area = "Identity" });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> UpdateUserPage(string id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }

        AppUser user = await _userManager.FindByIdAsync(id);
        ViewBag.user = ActiveUser;
        
        return View(user);
    }
    
    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateUser(string id, [FromForm] UpdateAppUserViewModel user)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        try
        {
            await _userRepository.UpdateUserAsync(id, user);
            ViewBag.user = ActiveUser;
            _notyfService.Success("User updated.");
            return RedirectToAction("UserDetails", "Identity", new { area = "Identity", id });
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _notyfService.Error($"There has been a exception while updating the user | DbUpdateConcurrencyException: {ex.Message}");
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
        catch (DbUpdateException ex)
        {
            _notyfService.Error($"There has been a exception while updating the user | DbUpdateException: {ex.Message}");
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            _notyfService.Error($"There has been a exception while updating the user | Exception: {ex.Message}");
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
        
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> UserDetails(string id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }

        AppUser user = await _userRepository.GetUserByIdAsync(id);
        ViewBag.user = ActiveUser;
        return View(user);
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        await _userRepository.DeleteUserAsync(id);
        _notyfService.Success("User deleted.");
        ViewBag.user = ActiveUser;
        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
    }
}