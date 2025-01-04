namespace Dash.Configuration;

public static class AppConfiguration
{
    public static IServiceCollection AddAppConfiguration(this IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddHttpContextAccessor();
        services.AddControllersWithViews();
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
        return services;
    }
}