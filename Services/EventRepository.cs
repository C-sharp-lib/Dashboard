using System.ComponentModel.Design;
using System.Drawing;
using Dash.Areas.Admin.Models;
using Dash.Areas.Identity.Models;
using Dash.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Z.EntityFramework.Plus;

namespace Dash.Services;

public class EventRepository : Repository<Event>, IEventRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Event> _dbSet;
    private readonly DbSet<UserEvents> _dbSetUserEvents;
    public EventRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<Event>();
        _dbSetUserEvents = _context.Set<UserEvents>();
    }
    public async Task<IEnumerable<Event>> GetAllEventsAsync()
    {
        var events = await _dbSet
            .Include(cu => cu.UserEvents)
            .ThenInclude(cu => cu.User)
            .ToListAsync();
        return events;
    }

    public async Task<IEnumerable<UserEvents>> GetAllUserEventsAsync()
    {
        return await _dbSetUserEvents
            .Include(cu => cu.User)
            .Include(cu => cu.Event)
            .ToListAsync();
    }

    public async Task<Event> GetEventByIdAsync(int id)
    {
        var e = await _dbSet
            .Include(eu => eu.UserEvents)
            .ThenInclude(eu => eu.User)
            .FirstOrDefaultAsync(eu => eu.EventId == id);
        if (e == null)
        {
            throw new NullReferenceException($"Event with id {id} was not found");
        }
        return e;
    }

    /*public async Task<UserEvents> GetUserEventByIdAsync(int id)
    {
        var e = await _dbSetUserEvents
            .Include(eu => eu.User)
            .Include(eu => eu.Event)
            .FirstOrDefaultAsync(eu => eu.UserEventId == id);
        if (e == null)
        {
            throw new NullReferenceException($"Event with id {id} was not found");
        }
        return e;
    }*/

    public async Task AddEventAsync([FromForm] EventViewModel events)
    {
        var eve = new Event
        {
            Title = events.Title,
            Description = events.Description,
            StartDate = events.StartDate,
            EndDate = events.EndDate,
            Color = events.Color,
            ForegroundColor = events.ForegroundColor
        };
        
        _dbSet.Add(eve);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateEventAsync([FromForm] UpdateEventViewModel events)
    {
        var eve = await _dbSet
            .Include(eu => eu.UserEvents)
            .ThenInclude(eu => eu.User)
            .FirstOrDefaultAsync(eu => eu.EventId == events.EventId);
        if (eve != null)
        {
            eve.Title = events.Title;
            eve.Description = events.Description;
            eve.StartDate = events.StartDate;
            eve.EndDate = events.EndDate;
            eve.Color = events.Color;
            eve.ForegroundColor = events.ForegroundColor;
            _dbSet.Update(eve);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new NullReferenceException($"Event with id {events.EventId} was not found");
        }
    }

    public async Task UpdateEventByIdAsync(int id, [FromForm] UpdateEventViewModel model)
    {
        var eve = await _dbSet
            .Include(eu => eu.UserEvents)
            .ThenInclude(eu => eu.User)
            .FirstOrDefaultAsync(eu => eu.EventId == id);
        if (eve != null)
        {
            eve.Title = model.Title;
            eve.Description = model.Description;
            eve.StartDate = model.StartDate;
            eve.EndDate = model.EndDate;
            eve.Color = model.Color;
            eve.ForegroundColor = model.ForegroundColor;
            _dbSet.Update(eve);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new NullReferenceException($"Event with id {eve?.EventId} was not found");
        }
    }


    public async Task DeleteEventAsync(int id)
    {
        var events = await _dbSet.FirstOrDefaultAsync(e => e.EventId == id);
        if (events == null)
        {
            throw new NullReferenceException($"Event with id {id} was not found");
        }
        _dbSet.Remove(events);
        await _context.SaveChangesAsync();
    }

    public async Task<int> EventCountAsync()
    {
        var events = await _dbSet.CountAsync();
        return events;
    }
    
}