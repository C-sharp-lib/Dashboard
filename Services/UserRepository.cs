using Dash.Areas.Identity.Models;
using Dash.Data;
using Microsoft.EntityFrameworkCore;

namespace Dash.Services;

public class UserRepository : Repository<AppUser>, IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<AppUser> _dbSet;
    public UserRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<AppUser>();
    }

    public Task<IEnumerable<AppUser>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<AppUser> GetUserByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task AddUserAsync(AppUserViewModel user)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserAsync(int id, AppUserViewModel user)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task CountUsersAsync()
    {
        throw new NotImplementedException();
    }
}