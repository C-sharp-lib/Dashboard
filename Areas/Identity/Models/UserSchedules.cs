using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dash.Areas.Identity.Models;

public class UserSchedules
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserScheduleId { get; set; }
    public string UserId { get; set; }
    public AppUser AppUser { get; set; }
    public int ScheduleId { get; set; }
    public Schedules Schedule { get; set; }
}