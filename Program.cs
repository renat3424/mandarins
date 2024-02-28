
using mandarinProject1.Services;
using mandarinProject1.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddTransient<IMailService, NullMailService>();
        builder.Services.AddControllersWithViews();
        builder.Services.AddControllersWithViews();

        builder.Services.AddScoped<IMandarinRepository, MandarinRepository>();
        builder.Services.AddHostedService<DestroyingService>();
        builder.Services.AddHostedService<CreatingService>();
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllerRoute("Default", "{controller}/{action}/{id?}", new { controller = "Home", action = "Index" });

        app.MapRazorPages();

        app.Run();
    }
}