using Pulse.Domain.Enums;

namespace Pulse.Domain.Entities;

/// <summary>
/// Categorizes applications into productivity buckets.
/// Users can assign any tracked app to a category.
/// </summary>
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ProductivityLevel Level { get; set; } = ProductivityLevel.Neutral;
    public string ColorHex { get; set; } = "#607D8B";

    /// <summary>
    /// Process names mapped to this category (e.g., "devenv", "rider64", "code").
    /// </summary>
    public List<AppCategoryMapping> Mappings { get; set; } = [];
}

/// <summary>
/// Maps a process name to a category so activity records inherit it automatically.
/// </summary>
public class AppCategoryMapping
{
    public int Id { get; set; }
    public string ProcessName { get; set; } = string.Empty;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}
