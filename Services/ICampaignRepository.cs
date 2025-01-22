using Dash.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dash.Services;

public interface ICampaignRepository : IRepository<Campaigns>
{
    Task<IEnumerable<Campaigns>> GetAllCampaignsAsync();
    Task<Campaigns> GetCampaignByIdAsync(int id);
    Task AddCampaignAsync([FromForm] AddCampaignViewModel model);
    Task UpdateCampaignAsync(int id, [FromForm] UpdateCampaignViewModel model);
    Task DeleteCampaignAsync(int id);
    Task<int> CampaignCountAsync();
}