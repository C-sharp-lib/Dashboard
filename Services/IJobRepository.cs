using Dash.Areas.Admin.Models;
using Dash.Areas.Identity.Models;
using EntityFrameworkCore.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dash.Services;

public interface IJobRepository : IRepository<Jobs>
{
    Task<IEnumerable<Jobs>> GetAllJobsAsync();
    Task<Jobs> GetJobByIdAsync(int id);
    Task AddJobAsync([FromForm] AddJobsViewModel model);
    Task UpdateJobAsync(int id, [FromForm] UpdateJobsViewModel model);
    Task DeleteJobAsync(int id);
    Task<int> CountJobsAsync();
}