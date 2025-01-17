using System.Text.Json;
using AspNetCoreHero.ToastNotification;
using Dash.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Dash.Data;
using Dash.Services;
using Microsoft.Extensions.FileProviders;

namespace Dash;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDatabaseConfiguration(builder.Configuration);
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddIdentityConfiguration();
        builder.Services.AddControllersWithViews();
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // Custom setting
        });
        builder.Services.AddAppConfiguration();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IEventRepository, EventRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddNotyf(config =>
        {
            config.DurationInSeconds = 10;
            config.IsDismissable = true;
            config.Position = NotyfPosition.TopCenter;
        });
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Areas", "Admin", "wwwroot")),
            RequestPath = "/Admin"
        });
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Areas", "Identity", "wwwroot")),
            RequestPath = "/Identity"
        });
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
            app.UseExceptionHandler(new ExceptionHandlerOptions()
            {
                AllowStatusCode404Response = true,
                ExceptionHandlingPath = "/Error"
            });
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
        }
        else
        {
            app.UseExceptionHandler("/Error/ExceptionHandler");
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseSession();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapAreaControllerRoute(
            name: "Identity",
            areaName: "Identity",
            pattern: "{area:exists}/{controller=Identity}/{action=Index}/{id?}");
        app.MapAreaControllerRoute(
            name: "Admin",
            areaName: "Admin",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}