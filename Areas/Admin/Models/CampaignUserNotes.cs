using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dash.Areas.Identity.Models;

namespace Dash.Areas.Admin.Models;

public class CampaignUserNotes
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CampaignUserNoteId { get; set; }
    public int CampaignId { get; set; }
    public Campaigns Campaign { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public string NoteTitle { get; set; }
    public string NoteContent { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}