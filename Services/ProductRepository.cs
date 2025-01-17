using Dash.Areas.Admin.Models;
using Dash.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace Dash.Services;

public class ProductRepository : Repository<Products>, IProductRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webhost;
    private readonly DbSet<Products> _dbSet;
    public ProductRepository(ApplicationDbContext context, IWebHostEnvironment webhost) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<Products>();
        _webhost = webhost;
    }

    public async Task<IEnumerable<Products>> GetAllProductsAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<Products> GetProductByIdAsync(int id)
    {
        var product = await _dbSet.FindAsync(id);
        if (product == null)
        {
            throw new NullReferenceException("Product not found");
        }
        return product;
    }

    public async Task AddProductAsync([FromForm] AddProductViewModel model)
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
            string uploadsFolder3 = Path.Combine(_webhost.WebRootPath, "Uploads/Products/");
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

        var product = new Products
        {
            Name = model.Name,
            Description = model.Description,
            ImageUrl = (uniqueFileName3 != null ? Path.Combine("Uploads/Products/", uniqueFileName3) : null)!,
            Category = model.Category,
            Currency = model.Currency,
            QuantityInStock = model.QuantityInStock,
            UPC = model.UPC,
            Price = model.Price
        };
        await _dbSet.AddAsync(product);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateProductAsync(int id, [FromForm] UpdateProductViewModel model)
    {
        
        Products product = await GetProductByIdAsync(id);
        string uploadsFolder = Path.Combine(_webhost.WebRootPath, "Uploads/Products/");
        if (model.ImageUrl != null && model.ImageUrl.Length > 0)
        {
            string uniqueFileName1 =
                Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImageUrl.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName1);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.ImageUrl.CopyToAsync(fileStream);
            }

            product.ImageUrl = uniqueFileName1;
        }

        product.Name = model.Name;
        product.Description = model.Description;
        product.Category = model.Category;
        product.Currency = model.Currency;
        product.QuantityInStock = model.QuantityInStock;
        product.UPC = model.UPC;
        product.Price = model.Price;
        product.UpdatedAt = DateTime.Now;
        _dbSet.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await GetProductByIdAsync(id);
        _dbSet.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountProductsAsync()
    {
        return await _dbSet.CountAsync();
    }
}