using Dash.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dash.Services;

public interface IProductRepository : IRepository<Products>
{
    Task<IEnumerable<Products>> GetAllProductsAsync();
    Task<Products> GetProductByIdAsync(int id);
    Task AddProductAsync([FromForm] AddProductViewModel model);
    Task UpdateProductAsync(int id, [FromForm] UpdateProductViewModel model);
    Task DeleteProductAsync(int id);
    Task<int> CountProductsAsync();
}