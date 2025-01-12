using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dash.Areas.Identity.Models;

namespace Dash.Areas.Admin.Models;

public class UserEvents
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserEventId { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; }
}