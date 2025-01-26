using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dash.Areas.Identity.Models;

namespace Dash.Areas.Admin.Models;

public class CampaignUserTasks
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CampaignUserTaskId { get; set; }
    [ForeignKey(nameof(CampaignId))]
    public int CampaignId { get; set; }
    public Campaigns Campaign { get; set; }
    [ForeignKey(nameof(UserId))]
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public string TaskTitle { get; set; }
    public string TaskDescription { get; set; }
    public string Priority { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime UpdatedAt { get; set; }

    public CampaignUserTasks()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
}