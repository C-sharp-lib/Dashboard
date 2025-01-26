using System.ComponentModel.DataAnnotations;
using Dash.Areas.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Dash.Areas.Admin.Models;

public class AddCampaignViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    [Precision(10,2)]
    public decimal Budget { get; set; }
    [Precision(10,2)]
    public decimal Spend { get; set; }
    public string TargetAudience { get; set; }
    public string Channel {  get; set; }
    public string Goals { get; set; }
    [Precision(10,2)]
    public decimal RevenueTarget { get; set; }
    [Precision(10, 2)]
    public decimal ActualRevenue { get; set; }
    public long Impressions { get; set; }
    public long Clicks { get; set; }
    public int LeadsGenerated { get; set; }
    public int Conversions {  get; set; }
    [Precision(10,2)]
    public decimal ROI { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class AddCampaignUserNotesViewModel : CampaignUserNotes
{
    public int CampaignId { get; set; }
    public Campaigns Campaign { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public string NoteTitle { get; set; }
    public string NoteContent { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class AddCampaignUserTasksViewModel : CampaignUserTasks
{
    public int CampaignId { get; set; }
    public Campaigns Campaign { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public string TaskTitle { get; set; }
    public string TaskDescription { get; set; }
    public string Priority { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class UpdateCampaignViewModel : Campaigns
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    [Precision(10,2)]
    public decimal Budget { get; set; }
    [Precision(10,2)]
    public decimal Spend { get; set; }
    public string TargetAudience { get; set; }
    public string Channel {  get; set; }
    public string Goals { get; set; }
    [Precision(10,2)]
    public decimal RevenueTarget { get; set; }
    [Precision(10, 2)]
    public decimal ActualRevenue { get; set; }
    public long Impressions { get; set; }
    public long Clicks { get; set; }
    public int LeadsGenerated { get; set; }
    public int Conversions {  get; set; }
    [Precision(10,2)]
    public decimal ROI { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public class UpdateCampaignUserNotesViewModel
{
    public int CampaignId { get; set; }
    public Campaigns Campaign { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public string NoteTitle { get; set; }
    public string NoteContent { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public class UpdateCampaignUserTasksViewModel
{
    [Required]
    public int CampaignId { get; set; }
    public Campaigns Campaign { get; set; }
    [Required]
    public string UserId { get; set; }
    public AppUser User { get; set; }
    [Required]
    public string TaskTitle { get; set; }
    [Required]
    public string TaskDescription { get; set; }
    [Required]
    public string Priority { get; set; }
    [Required]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    [Required]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}