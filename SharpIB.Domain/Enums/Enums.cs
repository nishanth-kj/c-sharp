namespace SharpIB.Domain.Enums;

public enum ProductivityLevel
{
    Productive,
    Neutral,
    Distracting
}

public enum GoalType
{
    /// <summary>Maximum time allowed (e.g., "Limit browser to 2 hours/day").</summary>
    Limit,
    /// <summary>Minimum time target (e.g., "Code for 4 hours/day").</summary>
    Target
}

public enum GoalStatus
{
    Active,
    Paused,
    Completed,
    Archived
}

public enum TrackingState
{
    Running,
    Paused,
    Idle
}

