using System.IO;
using System.Windows;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharpIB.Application.Services;
using SharpIB.Domain.Interfaces;
using SharpIB.Infrastructure.Data;
using SharpIB.Infrastructure.Repositories;
using SharpIB.Infrastructure.Services;
using SharpIB.UI.ViewModels;

namespace SharpIB.UI;

public partial class App : System.Windows.Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();

        // Database
        var dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SharpIB", "SharpIB.db");
        Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

        services.AddDbContext<SharpIBDbContext>(opt =>
            opt.UseSqlite($"Data Source={dbPath}"));

        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(SharpIB.Application.Commands.RecordActivityCommand).Assembly);
        });

        // Repositories
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IGoalRepository, GoalRepository>();
        services.AddScoped<IDailySnapshotRepository, DailySnapshotRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        // Services
        services.AddSingleton<IActiveWindowService, ActiveWindowService>();

        // ViewModels
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<ApplicationsViewModel>();
        services.AddTransient<GoalsViewModel>();
        services.AddTransient<ReportsViewModel>();
        services.AddSingleton<MainViewModel>();

        Services = services.BuildServiceProvider();

        // Auto-migrate database
        using (var scope = Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<SharpIBDbContext>();
            db.Database.EnsureCreated();
        }
    }
}

