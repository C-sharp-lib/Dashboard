using Dash.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dash.Services;

public interface ICustomerRepository : IRepository<Customers>
{
    Task<IEnumerable<Customers>> GetAllCustomersAsync();
    Task<Customers> GetCustomerByIdAsync(int id);
    Task AddCustomerAsync([FromForm] AddCustomerViewModel customer);
    Task UpdateCustomerAsync(int id, [FromForm] UpdateCustomerViewModel customer);
    Task DeleteCustomerAsync(int id);
    Task<int> CountCustomersAsync();   
}