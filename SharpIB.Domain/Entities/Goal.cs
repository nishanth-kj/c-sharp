using SharpIB.Domain.Enums;

namespace SharpIB.Domain.Entities;

/// <summary>
/// A productivity goal set by the user (e.g., "Code at least 4 hours/day"
/// or "Limit browser to 2 hours/day").
/// </summary>
public class Goal
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    /// <summary>Target or Limit — defines whether the user aims to reach or stay under the duration.</summary>
    public GoalType Type { get; set; }
    public GoalStatus Status { get; set; } = GoalStatus.Active;

    /// <summary>Target duration per day (e.g., 4 hours).</summary>
    public TimeSpan TargetDuration { get; set; }

    /// <summary>Comma-separated process names this goal tracks (e.g., "devenv,rider64,code").</summary>
    public string TrackedProcesses { get; set; } = string.Empty;

    /// <summary>Optional category to track instead of specific processes.</summary>
    public int? TrackedCategoryId { get; set; }
    public Category? TrackedCategory { get; set; }

    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

