using Microsoft.EntityFrameworkCore;

namespace Dash.Areas.Admin.Models;

public class AddProductViewModel
{
    public string Name { get; set; }
    public string UPC {  get; set; }
    public string Description { get; set; }
    [Precision(10,2)]
    public decimal Price { get; set; }
    public string Currency {  get; set; }
    public int QuantityInStock { get; set; } = 0;
    public string Category {  get; set; } = string.Empty;
    public IFormFile ImageUrl { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}