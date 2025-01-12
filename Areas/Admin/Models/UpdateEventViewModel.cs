using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dash.Areas.Admin.Models;

public class UpdateEventViewModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EventId { get; set; }
    [Required(ErrorMessage = "Title is required")]
    [MinLength(3, ErrorMessage = "Title must be at least 3 characters long")]
    [MaxLength(100, ErrorMessage = "Title must be less than 100 characters")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Description is required")]
    [MinLength(10, ErrorMessage = "Description must be at least 10 characters long")]
    [MaxLength(500, ErrorMessage = "Description must be less than 500 characters")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Start Date is required")]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = "End Date is required")]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    public DateTime EndDate { get; set; }
    [Required(ErrorMessage = "Color is required")]
    public string Color { get; set; }
    [Required(ErrorMessage = "Foreground Color is required")]
    public string ForegroundColor { get; set; }
}