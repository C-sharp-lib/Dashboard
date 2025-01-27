using Dash.Areas.Identity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dash.Areas.Admin.Models;

public class IndexViewModel
{
    public IEnumerable<AppUser> Users { get; set; }
    public IEnumerable<UserEvents> UserEvents { get; set; }
    public IEnumerable<Event> Events { get; set; }
    public IEnumerable<Products> Products { get; set; }
    public IEnumerable<Campaigns> Campaigns { get; set; }
    public IEnumerable<CampaignUserNotes> CampaignUserNotes { get; set; }
    public IEnumerable<CampaignUserTasks> CampaignUserTasks { get; set; }
    public IEnumerable<Jobs> Jobs { get; set; }
    public int CustomerCount { get; set; }

    public Event SelectedEvent { get; set; }
    public int UserCount { get; set; }
    public int EventCount { get; set; }
    public int ProductCount { get; set; }
    public int CampaignCount { get; set; }
    public int CampaignUserNoteCount { get; set; }
    public int CampaignUserTaskCount { get; set; }
    public int JobCount { get; set; }
}