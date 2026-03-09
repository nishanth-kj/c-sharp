using MediatR;
using SharpIB.Application.DTOs;
using SharpIB.Domain.Enums;
using SharpIB.Domain.Interfaces;

namespace SharpIB.Application.Queries;

// --- Get Today's Activities ---
public record GetTodayActivitiesQuery : IRequest<List<ActivityDto>>;

public class GetTodayActivitiesHandler(IActivityRepository repo)
    : IRequestHandler<GetTodayActivitiesQuery, List<ActivityDto>>
{
    public async Task<List<ActivityDto>> Handle(GetTodayActivitiesQuery request, CancellationToken ct)
    {
        var records = await repo.GetTodayAsync();
        return records.Select(r => new ActivityDto(
            r.Id, r.ProcessName, r.WindowTitle,
            r.StartTime, r.EndTime, r.Duration,
            r.Category?.Name, r.Category?.Level ?? ProductivityLevel.Neutral
        )).ToList();
    }
}

// --- Get Top Apps ---
public record GetTopAppsQuery(DateTime Start, DateTime End, int Count = 5) : IRequest<List<AppUsageSummaryDto>>;

public class GetTopAppsHandler(IActivityRepository repo)
    : IRequestHandler<GetTopAppsQuery, List<AppUsageSummaryDto>>
{
    public async Task<List<AppUsageSummaryDto>> Handle(GetTopAppsQuery request, CancellationToken ct)
    {
        var records = await repo.GetByDateRangeAsync(request.Start, request.End);
        return records
            .GroupBy(r => r.ProcessName)
            .Select(g => new AppUsageSummaryDto(
                g.Key,
                TimeSpan.FromTicks(g.Sum(r => r.Duration.Ticks)),
                g.Count(),
                g.First().Category?.Name,
                g.First().Category?.Level ?? ProductivityLevel.Neutral
            ))
            .OrderByDescending(a => a.TotalDuration)
            .Take(request.Count)
            .ToList();
    }
}

// --- Get Goal Progress ---
public record GetGoalProgressQuery : IRequest<List<GoalProgressDto>>;

public class GetGoalProgressHandler(IGoalRepository goalRepo, IActivityRepository activityRepo)
    : IRequestHandler<GetGoalProgressQuery, List<GoalProgressDto>>
{
    public async Task<List<GoalProgressDto>> Handle(GetGoalProgressQuery request, CancellationToken ct)
    {
        var goals = await goalRepo.GetAllActiveAsync();
        var today = DateTime.Today;
        var todayRecords = await activityRepo.GetByDateRangeAsync(today, today.AddDays(1));

        var results = new List<GoalProgressDto>();
        foreach (var goal in goals)
        {
            var trackedProcesses = goal.TrackedProcesses
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var matchingRecords = todayRecords
                .Where(r => trackedProcesses.Length == 0 || trackedProcesses.Contains(r.ProcessName, StringComparer.OrdinalIgnoreCase))
                .ToList();

            var currentDuration = TimeSpan.FromTicks(matchingRecords.Sum(r => r.Duration.Ticks));
            var progress = goal.TargetDuration.TotalMinutes > 0
                ? Math.Min(100, (currentDuration.TotalMinutes / goal.TargetDuration.TotalMinutes) * 100)
                : 0;

            var isMet = goal.Type == GoalType.Target
                ? currentDuration >= goal.TargetDuration
                : currentDuration <= goal.TargetDuration;

            results.Add(new GoalProgressDto(
                goal.Id, goal.Title, goal.Type, goal.TargetDuration,
                currentDuration, progress, goal.CurrentStreak, isMet));
        }

        return results;
    }
}

// --- Get Hourly Breakdown ---
public record GetHourlyBreakdownQuery(DateTime Date) : IRequest<List<HourlyUsageDto>>;

public class GetHourlyBreakdownHandler(IActivityRepository repo)
    : IRequestHandler<GetHourlyBreakdownQuery, List<HourlyUsageDto>>
{
    public async Task<List<HourlyUsageDto>> Handle(GetHourlyBreakdownQuery request, CancellationToken ct)
    {
        var records = await repo.GetByDateRangeAsync(request.Date.Date, request.Date.Date.AddDays(1));

        var hourly = Enumerable.Range(0, 24).Select(h =>
        {
            var hourStart = request.Date.Date.AddHours(h);
            var hourEnd = hourStart.AddHours(1);
            var duration = TimeSpan.FromTicks(
                records
                    .Where(r => r.StartTime < hourEnd && r.EndTime > hourStart)
                    .Sum(r =>
                    {
                        var effectiveStart = r.StartTime < hourStart ? hourStart : r.StartTime;
                        var effectiveEnd = r.EndTime > hourEnd ? hourEnd : r.EndTime;
                        return (effectiveEnd - effectiveStart).Ticks;
                    }));
            return new HourlyUsageDto(h, duration);
        }).ToList();

        return hourly;
    }
}

// --- Get All Apps Summary ---
public record GetAllAppsSummaryQuery(DateTime Start, DateTime End) : IRequest<List<AppUsageSummaryDto>>;

public class GetAllAppsSummaryHandler(IActivityRepository repo)
    : IRequestHandler<GetAllAppsSummaryQuery, List<AppUsageSummaryDto>>
{
    public async Task<List<AppUsageSummaryDto>> Handle(GetAllAppsSummaryQuery request, CancellationToken ct)
    {
        var records = await repo.GetByDateRangeAsync(request.Start, request.End);
        return records
            .GroupBy(r => r.ProcessName)
            .Select(g => new AppUsageSummaryDto(
                g.Key,
                TimeSpan.FromTicks(g.Sum(r => r.Duration.Ticks)),
                g.Count(),
                g.First().Category?.Name,
                g.First().Category?.Level ?? ProductivityLevel.Neutral
            ))
            .OrderByDescending(a => a.TotalDuration)
            .ToList();
    }
}

