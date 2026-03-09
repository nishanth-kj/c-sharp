namespace SharpIB.Domain.Entities;

/// <summary>
/// Pre-computed daily summary for fast dashboard and report rendering.
/// Generated at end-of-day or on demand.
/// </summary>
public class DailySnapshot
{
    public int Id { get; set; }
    public DateTime Date { get; set; }

    public TimeSpan TotalActiveTime { get; set; }
    public TimeSpan ProductiveTime { get; set; }
    public TimeSpan NeutralTime { get; set; }
    public TimeSpan DistractingTime { get; set; }

    /// <summary>0–100 productivity score based on category weights.</summary>
    public int ProductivityScore { get; set; }

    /// <summary>JSON blob of top 5 apps with durations for quick rendering.</summary>
    public string TopAppsJson { get; set; } = "[]";
}

