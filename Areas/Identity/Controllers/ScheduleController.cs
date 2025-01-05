using Dash.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dash.Areas.Identity.Controllers;

public class ScheduleController : Controller
{
    private readonly ApplicationDbContext _context;

    public ScheduleController(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> UserSchedule(string userId, int scheduleId)
    {
        var schedule = await _context.UserSchedules
            .Include(us => us.Schedule)
            .Include(us => us.AppUser)
            .Where(us => us.UserId == userId && us.ScheduleId == scheduleId)
            .FirstOrDefaultAsync();
        return View(schedule);
    }
}