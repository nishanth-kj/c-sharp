using SharpIB.Domain.Enums;

namespace SharpIB.Application.DTOs;

public record ActivityDto(
    int Id,
    string ProcessName,
    string WindowTitle,
    DateTime StartTime,
    DateTime EndTime,
    TimeSpan Duration,
    string? CategoryName,
    ProductivityLevel? Level);

public record AppUsageSummaryDto(
    string ProcessName,
    TimeSpan TotalDuration,
    int SessionCount,
    string? CategoryName,
    ProductivityLevel Level);

public record GoalProgressDto(
    int GoalId,
    string Title,
    GoalType Type,
    TimeSpan TargetDuration,
    TimeSpan CurrentDuration,
    double ProgressPercent,
    int CurrentStreak,
    bool IsMetToday);

public record DailyReportDto(
    DateTime Date,
    TimeSpan TotalActiveTime,
    TimeSpan ProductiveTime,
    TimeSpan DistractingTime,
    int ProductivityScore,
    List<AppUsageSummaryDto> TopApps);

public record HourlyUsageDto(int Hour, TimeSpan Duration);

