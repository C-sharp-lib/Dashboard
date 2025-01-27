using Dash.Areas.Admin.Models;
using Dash.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dash.Services;

public class JobRepository : Repository<Jobs>, IJobRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Jobs> _dbSet;
    public JobRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<Jobs>();
    }

    public async Task<IEnumerable<Jobs>> GetAllJobsAsync()
    {
        return await _dbSet
            .Include(c => c.User)
            .ToListAsync();
    }

    public async  Task<Jobs> GetJobByIdAsync(int id)
    {
        var job = await _dbSet.FirstOrDefaultAsync(x => x.JobId == id);
        if (job == null)
        {
            throw new NullReferenceException("Job not found");
        }
        return job;
    }

    public async Task AddJobAsync([FromForm] AddJobsViewModel model)
    {
        var job = new Jobs
        {
            Title = model.Title,
            Description = model.Description,
            UserId = model.UserId,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Status = model.Status,
            Priority = model.Priority,
            Type = model.Type,
            Category = model.Category,
            EstimatedCost = model.EstimatedCost
        };
        await _dbSet.AddAsync(job);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateJobAsync(int id, [FromForm] UpdateJobsViewModel model)
    {
        var job = await _dbSet.FirstOrDefaultAsync(x => x.JobId == id);
        if (job == null)
        {
            throw new NullReferenceException("Job not found");
        }
        job.Title = model.Title;
        job.Description = model.Description;
        job.UserId = model.UserId;
        job.StartDate = model.StartDate;
        job.EndDate = model.EndDate;
        job.Status = model.Status;
        job.Priority = model.Priority;
        job.Type = model.Type;
        job.Category = model.Category;
        job.EstimatedCost = model.EstimatedCost;
        _dbSet.Update(job);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteJobAsync(int id)
    {
        var job = await _dbSet.FirstOrDefaultAsync(x => x.JobId == id);
        if (job == null)
        {
            throw new NullReferenceException("Job not found");
        }
        _dbSet.Remove(job);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountJobsAsync()
    {
        return await _dbSet.CountAsync();
    }
}