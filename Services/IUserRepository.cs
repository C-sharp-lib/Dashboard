using Dash.Areas.Identity.Models;

namespace Dash.Services;

public interface IUserRepository : IRepository<AppUser>
{
    Task<IEnumerable<AppUser>> GetAllUsersAsync();
    Task<AppUser> GetUserByIdAsync(int id);
    Task AddUserAsync(AppUserViewModel user);
    Task UpdateUserAsync(int id, AppUserViewModel user);
    Task DeleteUserAsync(int id);
    Task CountUsersAsync();
}