using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MediatR;
using SharpIB.Application.DTOs;
using SharpIB.Application.Queries;
using SkiaSharp;

namespace SharpIB.UI.ViewModels;

public partial class ReportsViewModel : ObservableObject
{
    private readonly IMediator _mediator;

    [ObservableProperty] private DateTime _startDate = DateTime.Today.AddDays(-6);
    [ObservableProperty] private DateTime _endDate = DateTime.Today;
    [ObservableProperty] private string _totalTime = "0h 0m";
    [ObservableProperty] private string _avgPerDay = "0h 0m";
    [ObservableProperty] private int _avgScore;

    public ObservableCollection<ISeries> TrendSeries { get; } = [];
    public ObservableCollection<ISeries> CategorySeries { get; } = [];
    public ObservableCollection<AppUsageSummaryDto> TopApps { get; } = [];

    public Axis[] TrendXAxes { get; private set; } = [];
    public Axis[] TrendYAxes { get; } = [new Axis
    {
        TextSize = 10,
        LabelsPaint = new SolidColorPaint(SKColors.Gray),
        Labeler = v => $"{v:0}h"
    }];

    public ReportsViewModel(IMediator mediator) => _mediator = mediator;

    public async Task LoadDataAsync()
    {
        var days = (int)(EndDate - StartDate).TotalDays + 1;
        var labels = new string[days];
        var hours = new double[days];

        for (int i = 0; i < days; i++)
        {
            var date = StartDate.AddDays(i);
            labels[i] = date.ToString("MMM dd");

            var apps = await _mediator.Send(new GetAllAppsSummaryQuery(date, date.AddDays(1)));
            var totalTicks = apps.Sum(a => a.TotalDuration.Ticks);
            hours[i] = TimeSpan.FromTicks(totalTicks).TotalHours;
        }

        TrendXAxes = [new Axis
        {
            Labels = labels,
            TextSize = 10,
            LabelsPaint = new SolidColorPaint(SKColors.Gray),
            LabelsRotation = -45
        }];
        OnPropertyChanged(nameof(TrendXAxes));

        TrendSeries.Clear();
        TrendSeries.Add(new LineSeries<double>
        {
            Values = hours,
            Fill = new SolidColorPaint(SKColor.Parse("#7C4DFF").WithAlpha(40)),
            Stroke = new SolidColorPaint(SKColor.Parse("#7C4DFF")) { StrokeThickness = 3 },
            GeometryStroke = new SolidColorPaint(SKColor.Parse("#7C4DFF")) { StrokeThickness = 2 },
            GeometrySize = 8,
            LineSmoothness = 0.3
        });

        // Summary
        var totalHours = hours.Sum();
        var total = TimeSpan.FromHours(totalHours);
        TotalTime = $"{(int)total.TotalHours}h {total.Minutes}m";
        var avg = TimeSpan.FromHours(totalHours / Math.Max(1, days));
        AvgPerDay = $"{(int)avg.TotalHours}h {avg.Minutes}m";

        // Category breakdown
        var allApps = await _mediator.Send(new GetAllAppsSummaryQuery(StartDate, EndDate.AddDays(1)));
        var productive = allApps.Where(a => a.Level == Domain.Enums.ProductivityLevel.Productive).Sum(a => a.TotalDuration.TotalHours);
        var neutral = allApps.Where(a => a.Level == Domain.Enums.ProductivityLevel.Neutral).Sum(a => a.TotalDuration.TotalHours);
        var distracting = allApps.Where(a => a.Level == Domain.Enums.ProductivityLevel.Distracting).Sum(a => a.TotalDuration.TotalHours);

        CategorySeries.Clear();
        CategorySeries.Add(new PieSeries<double> { Values = [productive], Name = "Productive", Fill = new SolidColorPaint(SKColor.Parse("#4CAF50")) });
        CategorySeries.Add(new PieSeries<double> { Values = [neutral], Name = "Neutral", Fill = new SolidColorPaint(SKColor.Parse("#607D8B")) });
        CategorySeries.Add(new PieSeries<double> { Values = [distracting], Name = "Distracting", Fill = new SolidColorPaint(SKColor.Parse("#F44336")) });

        AvgScore = totalHours > 0 ? (int)(productive / totalHours * 100) : 0;

        // Top apps for period
        var top = await _mediator.Send(new GetTopAppsQuery(StartDate, EndDate.AddDays(1), 10));
        TopApps.Clear();
        foreach (var app in top) TopApps.Add(app);
    }

    [RelayCommand]
    private async Task Refresh() => await LoadDataAsync();

    partial void OnStartDateChanged(DateTime value) => _ = LoadDataAsync();
    partial void OnEndDateChanged(DateTime value) => _ = LoadDataAsync();
}

