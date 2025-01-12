using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dash.Areas.Admin.Models;

public class Event
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EventId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    public string Color { get; set; }
    public string ForegroundColor { get; set; }
    public IEnumerable<UserEvents> UserEvents { get; set; }
    
    public string TruncateWords(string text, int wordLimit)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }
        var words = text.Split(' ');
        if (words.Length <= wordLimit)
        {
            return text;
        }
        return string.Join(' ', words.Take(wordLimit)) + "...";
    }
}