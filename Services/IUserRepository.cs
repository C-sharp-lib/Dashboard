using Dash.Areas.Identity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dash.Services;

public interface IUserRepository : IRepository<AppUser>
{
    Task<IEnumerable<AppUser>> GetAllUsersAsync();
    Task<AppUser> GetUserByIdAsync(string id);
    Task AddUserAsync([FromForm] AppUserViewModel model);
    Task UpdateUserAsync(string id, [FromForm] UpdateAppUserViewModel user);
    Task DeleteUserAsync(string id);
    Task<int> CountUsersAsync();
}