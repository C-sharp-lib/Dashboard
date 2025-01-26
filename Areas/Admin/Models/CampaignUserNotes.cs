using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dash.Areas.Identity.Models;

namespace Dash.Areas.Admin.Models;

public class CampaignUserNotes
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CampaignUserNoteId { get; set; }
    [ForeignKey(nameof(CampaignId))]
    public int CampaignId { get; set; }
    public Campaigns Campaign { get; set; }
    [ForeignKey(nameof(UserId))]
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public string NoteTitle { get; set; }
    public string NoteContent { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime UpdatedAt { get; set; }

    public CampaignUserNotes()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
}