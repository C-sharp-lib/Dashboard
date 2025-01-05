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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<AppUser>().HasKey(x => x.Id);
        builder.Entity<Schedules>().HasKey(x => x.ScheduleId);

        builder.Entity<UserSchedules>()
            .HasOne(x => x.Schedule)
            .WithMany(x => x.UserSchedules)
            .HasForeignKey(x => x.ScheduleId);
        builder.Entity<UserSchedules>()
            .HasOne(u => u.AppUser)
            .WithMany(u => u.UserSchedules)
            .HasForeignKey(u => u.UserId);
    }
}