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
    public IdentityController(UserManager<AppUser> userM, SignInManager<AppUser> signInM, 
        IHttpContextAccessor contextAccessor, ApplicationDbContext context, IWebHostEnvironment webenv,
        IUserRepository userRepository)
    {
        _userManager = userM;
        _signInManager = signInM;
        _httpContextAccessor = contextAccessor;
        _context = context;
        _webenv = webenv;
        _userRepository = userRepository;
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
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User does not exist.");
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
                    throw new FormatException("Invalid image extension");
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
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Identity", new { area = "Identity" });
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateUser(string id)
    {
        if (ActiveUser == null)
        {
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }

        ViewBag.user = ActiveUser;
        ViewBag.updateUser = user;
        return View();
    }
    
    [HttpPost("{id}")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateUser(string id, [FromForm] UpdateAppUserViewModel user)
    {
        if (ActiveUser == null)
        {
            ViewBag.ErrorMessage = $"Could not access the page, please login to continue.";
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }


        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = $"Could not update the user with the ID: {id}";
                ModelState.AddModelError(string.Empty,
                    $"Could not update the user with the provided ID: {id} ModelState is not valid.");
            }
            await _userRepository.UpdateUserAsync(id, user);
            return RedirectToAction("UserDetails", "Identity", new { area = "Identity", id = id });
        }
        catch (DbUpdateConcurrencyException ex)
        {
            ViewBag.ErrorMessage = $"Could not update the user with the ID: {id}";
            ModelState.AddModelError(string.Empty,
                $"Could not update the user with the provided ID: {id} DbUpdateConcurrencyException: {ex.Message}");
        }
        catch (DbUpdateException ex)
        {
            ViewBag.ErrorMessage = $"Could not update the user with the ID: {id}";
            ModelState.AddModelError(string.Empty, $"Could not update the user with the provided ID: {id} DbUpdateException: {ex.Message}");
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"Could not update the user with the ID: {id}";
            ModelState.AddModelError(string.Empty, $"Could not update the user with the provided ID: {id} Exception: {ex.Message}");
        }
        ViewBag.user = ActiveUser;
        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
    }


    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> UserDetails(string id)
    {
        if (ActiveUser == null)
        {
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }

        AppUser user = await _userRepository.GetUserByIdAsync(id);
        ViewBag.user = ActiveUser;
        return View(user);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(string id)
    {
        await _userRepository.DeleteUserAsync(id);
        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
    }
}