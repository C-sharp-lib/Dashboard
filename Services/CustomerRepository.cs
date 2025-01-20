using Dash.Areas.Admin.Models;
using Dash.Data;
using Microsoft.EntityFrameworkCore;

namespace Dash.Services;

public class CustomerRepository : Repository<Customers>, ICustomerRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Customers> _dbSet;
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<Customers>();
    }

    public async Task<IEnumerable<Customers>> GetAllCustomersAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<Customers> GetCustomerByIdAsync(int id)
    {
        var customer = await _dbSet.FindAsync(id);
        if (customer == null)
        {
            throw new NullReferenceException("Customer not found");
        }
        return customer;
    }

    public async Task AddCustomerAsync(AddCustomerViewModel customer)
    {
        var customers = new Customers
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            UserName = customer.UserName,
            PhoneNumber = customer.PhoneNumber,
            Address = customer.Address,
            City = customer.City,
            State = customer.State,
            ZipCode = customer.ZipCode,
            Country = customer.Country
        };
        await _dbSet.AddAsync(customers);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCustomerAsync(int id, UpdateCustomerViewModel customer)
    {
        var cust = await _dbSet.FindAsync(id);
        if (cust == null)
        {
            throw new NullReferenceException("Customer not found");
        }
        cust.FirstName = customer.FirstName;
        cust.LastName = customer.LastName;
        cust.Email = customer.Email;
        cust.UserName = customer.UserName;
        cust.PhoneNumber = customer.PhoneNumber;
        cust.Address = customer.Address;
        cust.City = customer.City;
        cust.State = customer.State;
        cust.ZipCode = customer.ZipCode;
        cust.Country = customer.Country;
        _dbSet.Update(cust);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCustomerAsync(int id)
    {
        var cust = await _dbSet.FindAsync(id);
        if (cust == null)
        {
            throw new NullReferenceException("Customer not found");
        }
        _dbSet.Remove(cust);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountCustomersAsync()
    {
        return await _dbSet.CountAsync();
    }
}