using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dash.Areas.Admin.Models;

public class Campaigns
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CampaignId { get; set; }
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
    public DateTime CreatedAt { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime UpdatedAt { get; set; }
    public ICollection<CampaignUserNotes> CampaignUserNotes { get; set; }
    public ICollection<CampaignUserTasks> CampaignUserTasks { get; set; }
        
    public string TruncateWords(string text, int wordCount)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;
        var words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (words.Length <= wordCount) return text;
        return string.Join(" ", words.Take(wordCount)) + "...";
    }
    public Campaigns()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
}