using Dash.Areas.Admin.Models;
using Dash.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dash.Services;

public class CampaignUserNoteRepository : Repository<CampaignUserNotes>, ICampaignUserNoteRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<CampaignUserNotes> _dbSet;
    public CampaignUserNoteRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
        _dbSet = context.Set<CampaignUserNotes>();
    }

    public async Task<IEnumerable<CampaignUserNotes>> GetAllCampaignUserNotesAsync()
    {
        return await _dbSet
            .Include(c => c.Campaign)
            .Include(c => c.User)
            .ToListAsync();
    }

    public async Task<CampaignUserNotes> GetCampaignUserNoteByIdAsync(int id)
    {
        var campaignUserNotes = await _dbSet
            .Include(c => c.Campaign)
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.CampaignUserNoteId == id);
        if (campaignUserNotes == null)
        {
            throw new NullReferenceException("Campaign user note could not be found");
        }
        return campaignUserNotes;
    }

    public async Task AddCampaignUserNoteAsync([FromForm] AddCampaignUserNotesViewModel model)
    {
        CampaignUserNotes campaignUserNotes = new CampaignUserNotes
        {
            CampaignId = model.CampaignId,
            UserId = model.UserId,
            NoteTitle = model.NoteTitle,
            NoteContent = model.NoteContent
        };
        _dbSet.Add(campaignUserNotes);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCampaignUserNoteAsync(int id, [FromForm] UpdateCampaignUserNotesViewModel model)
    {
        var campaignUserNotes = await GetCampaignUserNoteByIdAsync(id);
        campaignUserNotes.NoteTitle = model.NoteTitle;
        campaignUserNotes.NoteContent = model.NoteContent;
        campaignUserNotes.CampaignId = model.CampaignId;
        campaignUserNotes.UserId = model.UserId;
        _dbSet.Update(campaignUserNotes);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCampaignUserNoteAsync(int id)
    {
        var campaignUserNotes = await GetCampaignUserNoteByIdAsync(id);
        _dbSet.Remove(campaignUserNotes);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CampaignUserNoteCountAsync()
    {
        return await _dbSet.CountAsync();
    }
}