using Dash.Areas.Admin.Models;
using Dash.Data;
using Microsoft.EntityFrameworkCore;

namespace Dash.Services;

public class CampaignRepository : Repository<Campaigns>, ICampaignRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Campaigns> _dbSet;
    public CampaignRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<Campaigns>();
    }

    public async Task<IEnumerable<Campaigns>> GetAllCampaignsAsync()
    {
        return await _dbSet
            .Include(c => c.CampaignUserNotes)
            .ThenInclude(c => c.User)
            .Include(ct => ct.CampaignUserTasks)
            .ThenInclude(c => c.User)
            .ToListAsync();
    }

    public async Task<Campaigns> GetCampaignByIdAsync(int id)
    {
        var campaign = await _dbSet
            .Include(c => c.CampaignUserNotes)
            .ThenInclude(c => c.User)
            .Include(ct => ct.CampaignUserTasks)
            .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(c => c.CampaignId == id);
        if (campaign == null)
        {
            throw new NullReferenceException($"Campaign with id {id} was not found");
        }
        return campaign;
    }

    public async Task AddCampaignAsync(AddCampaignViewModel model)
    {
        Campaigns campaign = new Campaigns
        {
            Title = model.Title,
            Description = model.Description,
            Type = model.Type,
            Status = model.Status,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Budget = model.Budget,
            Spend = model.Spend,
            TargetAudience = model.TargetAudience,
            Channel = model.Channel,
            Goals = model.Goals,
            RevenueTarget = model.RevenueTarget,
            ActualRevenue = model.ActualRevenue,
            Impressions = model.Impressions,
            Clicks = model.Clicks,
            LeadsGenerated = model.LeadsGenerated,
            Conversions = model.Conversions,
            ROI = model.ROI,
        };
        _dbSet.Add(campaign);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCampaignAsync(int id, UpdateCampaignViewModel model)
    {
        var campaign = await _dbSet.FirstOrDefaultAsync(c => c.CampaignId == id);
        if (campaign == null)
        {
            throw new NullReferenceException($"Campaign with id {id} was not found");
        }
        campaign.Title = model.Title;
        campaign.Description = model.Description;
        campaign.Type = model.Type;
        campaign.Status = model.Status;
        campaign.StartDate = model.StartDate;
        campaign.EndDate = model.EndDate;
        campaign.Budget = model.Budget;
        campaign.Spend = model.Spend;
        campaign.TargetAudience = model.TargetAudience;
        campaign.Channel = model.Channel;
        campaign.Goals = model.Goals;
        campaign.RevenueTarget = model.RevenueTarget;
        campaign.ActualRevenue = model.ActualRevenue;
        campaign.Impressions = model.Impressions;
        campaign.Clicks = model.Clicks;
        campaign.LeadsGenerated = model.LeadsGenerated;
        campaign.Conversions = model.Conversions;
        campaign.ROI = model.ROI;
        _dbSet.Update(campaign);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCampaignAsync(int id)
    {
        var campaign = await _dbSet.FirstOrDefaultAsync(c => c.CampaignId == id);
        if (campaign == null)
        {
            throw new NullReferenceException($"Campaign with id {id} was not found");
        }
        _dbSet.Remove(campaign);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CampaignCountAsync()
    {
        return await _dbSet.CountAsync();
    }
}