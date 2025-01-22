using Dash.Areas.Admin.Models;
using Dash.Data;
using Microsoft.EntityFrameworkCore;

namespace Dash.Services;

public class CampaignUserTaskRepository : Repository<CampaignUserTasks>, ICampaignUserTaskRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<CampaignUserTasks> _dbSet;
    public CampaignUserTaskRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
        _dbSet = context.Set<CampaignUserTasks>();
    }

    public async Task<IEnumerable<CampaignUserTasks>> GetAllCampaignUserTasksAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<CampaignUserTasks> GetCampaignUserTaskByIdAsync(int id)
    {
        var campaignUserTasks = await _dbSet.FirstOrDefaultAsync(c => c.CampaignUserTaskId == id);
        if (campaignUserTasks == null)
        {
            throw new NullReferenceException("Campaign user task not found");
        }
        return campaignUserTasks;
    }

    public async Task AddCampaignUserTaskAsync(AddCampaignUserTasksViewModel model)
    {
        var campaignUserTask = new CampaignUserTasks
        {
            CampaignId = model.CampaignId,
            UserId = model.UserId,
            TaskTitle = model.TaskTitle,
            TaskDescription = model.TaskDescription,
            Priority = model.Priority,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
        };
        _dbSet.Add(campaignUserTask);
        await _context.SaveChangesAsync();
        
    }

    public async Task UpdateCampaignUserTaskAsync(int id, UpdateCampaignUserTasksViewModel model)
    {
        var campaignUserTask = await _dbSet.FirstOrDefaultAsync(c => c.CampaignUserTaskId == id);
        if (campaignUserTask == null)
        {
            throw new NullReferenceException("Campaign user task not found");
        }
        campaignUserTask.TaskTitle = model.TaskTitle;
        campaignUserTask.TaskDescription = model.TaskDescription;
        campaignUserTask.Priority = model.Priority;
        campaignUserTask.StartDate = model.StartDate;
        campaignUserTask.EndDate = model.EndDate;
        campaignUserTask.CampaignId = model.CampaignId;
        campaignUserTask.UserId = model.UserId;
        _context.Update(campaignUserTask);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCampaignUserTaskAsync(int id)
    {
        var campaignUserTask = await _dbSet.FirstOrDefaultAsync(c => c.CampaignUserTaskId == id);
        if (campaignUserTask == null)
        {
            throw new NullReferenceException("Campaign user task not found");
        }
        _dbSet.Remove(campaignUserTask);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CampaignUserTaskCountAsync()
    {
        return await _dbSet.CountAsync();
    }
}