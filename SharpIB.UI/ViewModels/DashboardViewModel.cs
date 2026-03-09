using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MediatR;
using SharpIB.Application.DTOs;
using SharpIB.Application.Queries;
using SkiaSharp;

namespace SharpIB.UI.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    private readonly IMediator _mediator;

    [ObservableProperty] private string _totalScreenTime = "0h 0m";
    [ObservableProperty] private string _productiveTime = "0h 0m";
    [ObservableProperty] private string _distractingTime = "0h 0m";
    [ObservableProperty] private int _productivityScore;
    [ObservableProperty] private int _appsUsed;

    public ObservableCollection<GoalProgressDto> GoalProgress { get; } = [];
    public ObservableCollection<ISeries> TopAppsSeries { get; } = [];
    public ObservableCollection<ISeries> HourlySeries { get; } = [];
    public Axis[] HourlyXAxes { get; } = [new Axis
    {
        Labels = Enumerable.Range(0, 24).Select(h => $"{h:00}").ToArray(),
        LabelsRotation = 0,
        TextSize = 10,
        LabelsPaint = new SolidColorPaint(SKColors.Gray)
    }];
    public Axis[] HourlyYAxes { get; } = [new Axis
    {
        TextSize = 10,
        LabelsPaint = new SolidColorPaint(SKColors.Gray),
        Labeler = v => $"{v:0}m"
    }];

    public DashboardViewModel(IMediator mediator) => _mediator = mediator;

    public async Task LoadDataAsync()
    {
        var today = DateTime.Today;

        // Top apps
        var topApps = await _mediator.Send(new GetTopAppsQuery(today, today.AddDays(1), 6));
        TopAppsSeries.Clear();
        if (topApps.Any())
        {
            TopAppsSeries.Add(new ColumnSeries<double>
            {
                Values = topApps.Select(a => a.TotalDuration.TotalMinutes).ToArray(),
                Fill = new SolidColorPaint(SKColor.Parse("#7C4DFF")),
                Rx = 4,
                Ry = 4,
                MaxBarWidth = 40
            });
        }

        // Hourly breakdown
        var hourly = await _mediator.Send(new GetHourlyBreakdownQuery(today));
        HourlySeries.Clear();
        HourlySeries.Add(new ColumnSeries<double>
        {
            Values = hourly.Select(h => h.Duration.TotalMinutes).ToArray(),
            Fill = new SolidColorPaint(SKColor.Parse("#00BFA5")),
            Rx = 3,
            Ry = 3,
            MaxBarWidth = 20
        });

        // Goal progress
        var goals = await _mediator.Send(new GetGoalProgressQuery());
        GoalProgress.Clear();
        foreach (var g in goals) GoalProgress.Add(g);

        // Summary stats
        var allApps = await _mediator.Send(new GetAllAppsSummaryQuery(today, today.AddDays(1)));
        var totalTicks = allApps.Sum(a => a.TotalDuration.Ticks);
        var total = TimeSpan.FromTicks(totalTicks);
        TotalScreenTime = $"{(int)total.TotalHours}h {total.Minutes}m";
        AppsUsed = allApps.Count;

        var productive = TimeSpan.FromTicks(allApps
            .Where(a => a.Level == Domain.Enums.ProductivityLevel.Productive)
            .Sum(a => a.TotalDuration.Ticks));
        var distracting = TimeSpan.FromTicks(allApps
            .Where(a => a.Level == Domain.Enums.ProductivityLevel.Distracting)
            .Sum(a => a.TotalDuration.Ticks));

        ProductiveTime = $"{(int)productive.TotalHours}h {productive.Minutes}m";
        DistractingTime = $"{(int)distracting.TotalHours}h {distracting.Minutes}m";

        ProductivityScore = totalTicks > 0
            ? (int)(productive.Ticks * 100 / totalTicks)
            : 0;
    }
}

