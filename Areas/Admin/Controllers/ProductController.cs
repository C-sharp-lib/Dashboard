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
public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webhost;
    private readonly IProductRepository _productRepository;
    private readonly INotyfService _notyfService;
    public ProductController(ApplicationDbContext context, IWebHostEnvironment webhost, IProductRepository productRepository, INotyfService notyfService)
    {
        _context = context;
        _webhost = webhost;
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
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        var products = await _productRepository.GetAllProductsAsync();
        ViewBag.user = ActiveUser;
        return View(products);
    }
    [HttpGet]
    public async Task<IActionResult> AddProductPage()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        ViewBag.user = ActiveUser;
        return View();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ProductDetails(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        var product = await _productRepository.GetProductByIdAsync(id);
        ViewBag.user = ActiveUser;
        return View(product);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> UpdateProductPage(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        var product = await _productRepository.GetProductByIdAsync(id);
        ViewBag.product = product;
        ViewBag.user = ActiveUser;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddProduct([FromForm] AddProductViewModel model)
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
                await _productRepository.AddProductAsync(model);
                _notyfService.Success("Product added.");
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            
        }
        catch (Exception ex)
        {
            _notyfService.Error($"There was an error adding the product | Exception: {ex.Message}");
            return RedirectToAction("AddProductPage", "Product", new { area = "Admin" });
        }
        ViewBag.user = ActiveUser;
        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProduct(int id, [FromForm] UpdateProductViewModel model)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        var product = await _productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            _notyfService.Error("Product not found.");
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
        try
        {
            await _productRepository.UpdateProductAsync(id, model);
            ViewBag.user = ActiveUser;
            _notyfService.Success("Product updated.");
            return RedirectToAction("ProductDetails", "Product", new { area = "Admin", id = product?.ProductId});
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _notyfService.Error($"There has been a exception while updating the product | DbUpdateConcurrencyException: {ex.Message}");
            return RedirectToAction("ProductDetails", "Product", new { area = "Admin", id = product?.ProductId});
        }
        catch (DbUpdateException ex)
        {
            _notyfService.Error($"There has been a exception while updating the product | DbUpdateException: {ex.Message}");
            return RedirectToAction("ProductDetails", "Product", new { area = "Admin", id = product?.ProductId});
        }
        catch (Exception ex)
        {
            _notyfService.Error($"There has been a exception while updating the product | Exception: {ex.Message}");
            return RedirectToAction("ProductDetails", "Product", new { area = "Admin", id = product?.ProductId});
        }
        
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        await _productRepository.DeleteProductAsync(id);
        _notyfService.Success("Product deleted.");
        ViewBag.user = ActiveUser;
        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
    }
}