using Dash.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dash.Services;

public interface ICampaignUserTaskRepository : IRepository<CampaignUserTasks>
{
    Task<IEnumerable<CampaignUserTasks>> GetAllCampaignUserTasksAsync();
    Task<CampaignUserTasks> GetCampaignUserTaskByIdAsync(int id);
    Task AddCampaignUserTaskAsync([FromForm] AddCampaignUserTasksViewModel model);
    Task UpdateCampaignUserTaskAsync(int id, [FromForm] UpdateCampaignUserTasksViewModel model);
    Task DeleteCampaignUserTaskAsync(int id);
    Task<int> CampaignUserTaskCountAsync();
}