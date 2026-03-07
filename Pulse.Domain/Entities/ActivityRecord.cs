using Pulse.Domain.Enums;

namespace Pulse.Domain.Entities;

/// <summary>
/// Represents a single tracked usage session of an application window.
/// Each record captures the time span a user spent in a particular window.
/// </summary>
public class ActivityRecord
{
    public int Id { get; set; }
    public string ProcessName { get; set; } = string.Empty;
    public string WindowTitle { get; set; } = string.Empty;
    public string ExecutablePath { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration => EndTime - StartTime;

    /// <summary>Foreign key to the assigned category. Null if uncategorized.</summary>
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
}
