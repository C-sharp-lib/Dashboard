using Dash.Areas.Identity.Models;
using Dash.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dash.Services;

public class UserRepository : Repository<AppUser>, IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<AppUser> _dbSet;
    private readonly IWebHostEnvironment _webenv;
    private readonly UserManager<AppUser> _userManager;

    public UserRepository(ApplicationDbContext context, IWebHostEnvironment webenv, UserManager<AppUser> userManager) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<AppUser>();
        _webenv = webenv;
        _userManager = userManager;
    }

    public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
    {
        return await _dbSet
            .Include(u => u.UserEvents)
            .ThenInclude(ue => ue.Event)
            .ToListAsync();
    }

    public async Task<AppUser> GetUserByIdAsync(string id)
    {
        var user = await _dbSet.FindAsync(id);
        if (user == null)
        {
            throw new NullReferenceException("User not found");
        }

        return user;
    }

    public async Task AddUserAsync([FromForm] AppUserViewModel model)
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
            string uploadsFolder3 = Path.Combine(_webenv.WebRootPath, "Uploads/AppUsers/");
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

        var user = new AppUser
        {
            UserName = model.UserName,
            Email = model.Email,
            FirstName = model.FirstName,
            MiddleName = model.MiddleName,
            LastName = model.LastName,
            DateOfBirth = model.DateOfBirth,
            DateOfHire = model.DateOfHire,
            Address = model.Address,
            City = model.City,
            State = model.State,
            ZipCode = model.ZipCode,
            ImageUrl = (uniqueFileName3 != null ? Path.Combine("Uploads/AppUsers/", uniqueFileName3) : null)!
        };
        await _userManager.CreateAsync(user, model.Password);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(string id, [FromForm] UpdateAppUserViewModel user)
    {
        AppUser userToUpdate = await GetUserByIdAsync(id);
        string uploadsFolder = Path.Combine(_webenv.WebRootPath, "Uploads/AppUsers/");
        if (user.ImageUrl != null && user.ImageUrl.Length > 0)
        {
            string uniqueFileName1 =
                Guid.NewGuid().ToString() + "_" + Path.GetFileName(user.ImageUrl.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName1);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await user.ImageUrl.CopyToAsync(fileStream);
            }

            userToUpdate.ImageUrl = uniqueFileName1;
        }

        userToUpdate.FirstName = user.FirstName;
        userToUpdate.MiddleName = user.MiddleName;
        userToUpdate.LastName = user.LastName;
        userToUpdate.Email = user.Email;
        userToUpdate.UserName = user.UserName;
        userToUpdate.DateOfBirth = user.DateOfBirth;
        userToUpdate.DateOfHire = user.DateOfHire;
        userToUpdate.PhoneNumber = user.PhoneNumber;
        userToUpdate.Address = user.Address;
        userToUpdate.City = user.City;
        userToUpdate.State = user.State;
        userToUpdate.ZipCode = user.ZipCode;
        userToUpdate.PhoneNumberConfirmed = true;
        await _userManager.UpdateAsync(userToUpdate);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(string id)
    {
        AppUser userToDelete = await GetUserByIdAsync(id);
        _dbSet.Remove(userToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountUsersAsync()
    {
        return await _dbSet.CountAsync();
    }
}