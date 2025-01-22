using Dash.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dash.Services;

public interface ICampaignUserNoteRepository : IRepository<CampaignUserNotes>
{
    Task<IEnumerable<CampaignUserNotes>> GetAllCampaignUserNotesAsync();
    Task<CampaignUserNotes> GetCampaignUserNoteByIdAsync(int id);
    Task AddCampaignUserNoteAsync([FromForm] AddCampaignUserNotesViewModel model);
    Task UpdateCampaignUserNoteAsync(int id, [FromForm] UpdateCampaignUserNotesViewModel model);
    Task DeleteCampaignUserNoteAsync(int id);
    Task<int> CampaignUserNoteCountAsync();
}