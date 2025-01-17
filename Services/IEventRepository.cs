using Dash.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dash.Services;

public interface IEventRepository : IRepository<Event>
{
    Task<IEnumerable<Event>> GetAllEventsAsync();
    Task<IEnumerable<UserEvents>> GetAllUserEventsAsync();
    Task<Event> GetEventByIdAsync(int id);
    Task AddEventAsync([FromForm] EventViewModel model);
    Task UpdateEventAsync([FromForm] UpdateEventViewModel model);
    Task UpdateEventByIdAsync(int id, [FromForm] UpdateEventViewModel model);
    Task DeleteEventAsync(int id);
    Task<int> EventCountAsync();
}