using System.IO;
using System.Windows;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pulse.Application.Services;
using Pulse.Domain.Interfaces;
using Pulse.Infrastructure.Data;
using Pulse.Infrastructure.Repositories;
using Pulse.Infrastructure.Services;
using Pulse.UI.ViewModels;

namespace Pulse.UI;

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
            "Pulse", "pulse.db");
        Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

        services.AddDbContext<PulseDbContext>(opt =>
            opt.UseSqlite($"Data Source={dbPath}"));

        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Pulse.Application.Commands.RecordActivityCommand).Assembly);
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
            var db = scope.ServiceProvider.GetRequiredService<PulseDbContext>();
            db.Database.EnsureCreated();
        }
    }
}
