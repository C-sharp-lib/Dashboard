using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dash.Areas.Admin.Models;

public class UpdateProductViewModel
{
 
    public string Name { get; set; }
    public string UPC {  get; set; }
    public string Description { get; set; }
    [Precision(10,2)]
    public decimal Price { get; set; }
    public string Currency {  get; set; }
    public int QuantityInStock { get; set; }
    public string Category {  get; set; }
    public IFormFile ImageUrl { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

}