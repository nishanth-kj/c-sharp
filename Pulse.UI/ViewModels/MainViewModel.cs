using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Pulse.Application.Commands;
using Pulse.Application.Services;

namespace Pulse.UI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IActiveWindowService _windowService;
    private readonly DispatcherTimer _trackingTimer;
    private readonly DispatcherTimer _clockTimer;

    private string _lastProcessName = string.Empty;
    private string _lastWindowTitle = string.Empty;
    private DateTime _lastSwitchTime = DateTime.Now;

    [ObservableProperty] private string _currentPage = "Dashboard";
    [ObservableProperty] private string _trackingStatus = "Tracking";
    [ObservableProperty] private bool _isTracking = true;
    [ObservableProperty] private string _currentTime = DateTime.Now.ToString("HH:mm:ss");
    [ObservableProperty] private string _currentDate = DateTime.Now.ToString("dddd, MMMM dd");
    [ObservableProperty] private string _activeAppName = "—";
    [ObservableProperty] private string _sessionDuration = "00:00:00";
    [ObservableProperty] private object? _currentViewModel;

    private DateTime _sessionStart = DateTime.Now;

    public MainViewModel(IActiveWindowService windowService)
    {
        _windowService = windowService;

        // Tracking timer — polls every 3 seconds
        _trackingTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
        _trackingTimer.Tick += OnTrackingTick;
        _trackingTimer.Start();

        // Clock timer — updates every second
        _clockTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _clockTimer.Tick += (_, _) =>
        {
            CurrentTime = DateTime.Now.ToString("HH:mm:ss");
            CurrentDate = DateTime.Now.ToString("dddd, MMMM dd");
            SessionDuration = (DateTime.Now - _sessionStart).ToString(@"hh\:mm\:ss");
        };
        _clockTimer.Start();

        NavigateTo("Dashboard");
    }

    [RelayCommand]
    private void NavigateTo(string page)
    {
        CurrentPage = page;
        CurrentViewModel = page switch
        {
            "Dashboard" => App.Services.GetRequiredService<DashboardViewModel>(),
            "Applications" => App.Services.GetRequiredService<ApplicationsViewModel>(),
            "Goals" => App.Services.GetRequiredService<GoalsViewModel>(),
            "Reports" => App.Services.GetRequiredService<ReportsViewModel>(),
            _ => CurrentViewModel
        };

        // Trigger data load for the navigated page
        if (CurrentViewModel is DashboardViewModel dashboard) _ = dashboard.LoadDataAsync();
        if (CurrentViewModel is ApplicationsViewModel apps) _ = apps.LoadDataAsync();
        if (CurrentViewModel is GoalsViewModel goals) _ = goals.LoadDataAsync();
        if (CurrentViewModel is ReportsViewModel reports) _ = reports.LoadDataAsync();
    }

    [RelayCommand]
    private void ToggleTracking()
    {
        IsTracking = !IsTracking;
        TrackingStatus = IsTracking ? "Tracking" : "Paused";
        if (IsTracking)
        {
            _lastSwitchTime = DateTime.Now;
            _trackingTimer.Start();
        }
        else
        {
            _trackingTimer.Stop();
            _ = FlushCurrentActivityAsync();
        }
    }

    private async void OnTrackingTick(object? sender, EventArgs e)
    {
        if (!IsTracking) return;

        var processName = _windowService.GetActiveProcessName();
        var windowTitle = _windowService.GetActiveWindowTitle();

        if (string.IsNullOrEmpty(processName)) return;

        ActiveAppName = processName;

        // Detect window switch
        if (processName != _lastProcessName || windowTitle != _lastWindowTitle)
        {
            await FlushCurrentActivityAsync();
            _lastProcessName = processName;
            _lastWindowTitle = windowTitle;
            _lastSwitchTime = DateTime.Now;
        }
    }

    private async Task FlushCurrentActivityAsync()
    {
        if (string.IsNullOrEmpty(_lastProcessName)) return;
        var duration = DateTime.Now - _lastSwitchTime;
        if (duration.TotalSeconds < 2) return; // Ignore tiny blips

        try
        {
            using var scope = App.Services.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new RecordActivityCommand(
                _lastProcessName,
                _lastWindowTitle,
                _windowService.GetActiveExecutablePath(),
                _lastSwitchTime,
                DateTime.Now));
        }
        catch
        {
            // Swallow — tracking should never crash the app
        }
    }
}
