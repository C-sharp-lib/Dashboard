using Dash.Areas.Admin.Models;
using Dash.Areas.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dash.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Schedules> Schedules { get; set; }
    public DbSet<UserSchedules> UserSchedules { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<UserEvents> UserEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<AppUser>().HasKey(x => x.Id);
        builder.Entity<Schedules>().HasKey(x => x.ScheduleId);
        builder.Entity<Event>().HasKey(x => x.EventId);
        builder.Entity<UserEvents>().HasKey(x => new { x.UserId, x.EventId, x.UserEventId });
        builder.Entity<UserSchedules>().HasKey(x => new { x.UserId, x.ScheduleId, x.UserScheduleId });
        builder.Entity<UserSchedules>()
            .HasOne(x => x.Schedule)
            .WithMany(x => x.UserSchedules)
            .HasForeignKey(x => x.ScheduleId);
        builder.Entity<UserSchedules>()
            .HasOne(u => u.AppUser)
            .WithMany(u => u.UserSchedules)
            .HasForeignKey(u => u.UserId);
        builder.Entity<UserEvents>()
            .HasOne(x => x.Event)
            .WithMany(x => x.UserEvents)
            .HasForeignKey(x => x.EventId);
        builder.Entity<UserEvents>()
            .HasOne(x => x.User)
            .WithMany(x => x.UserEvents)
            .HasForeignKey(x => x.UserId);
    }
}