using System.Text.Json.Serialization;

namespace site.Models;

public class SiteConfig
{
    [JsonPropertyName("appName")] public string AppName { get; set; } = "";
    [JsonPropertyName("tagline")] public string Tagline { get; set; } = "";
    [JsonPropertyName("heroTitle")] public string HeroTitle { get; set; } = "";
    [JsonPropertyName("heroSubtitle")] public string HeroSubtitle { get; set; } = "";
    [JsonPropertyName("heroBadge")] public string HeroBadge { get; set; } = "";
    [JsonPropertyName("repo")] public string Repo { get; set; } = "";
    [JsonPropertyName("githubUrl")] public string GithubUrl { get; set; } = "";
    [JsonPropertyName("docsUrl")] public string DocsUrl { get; set; } = "";
    [JsonPropertyName("changelogUrl")] public string ChangelogUrl { get; set; } = "";
    [JsonPropertyName("licenseUrl")] public string LicenseUrl { get; set; } = "";
    [JsonPropertyName("releasesUrl")] public string ReleasesUrl { get; set; } = "";
    [JsonPropertyName("ctaTitle")] public string CtaTitle { get; set; } = "";
    [JsonPropertyName("ctaSubtitle")] public string CtaSubtitle { get; set; } = "";
    [JsonPropertyName("footerText")] public string FooterText { get; set; } = "";
}

public class FeatureItem
{
    [JsonPropertyName("icon")] public string Icon { get; set; } = "";
    [JsonPropertyName("title")] public string Title { get; set; } = "";
    [JsonPropertyName("description")] public string Description { get; set; } = "";
    [JsonPropertyName("accentColor")] public string AccentColor { get; set; } = "";
    [JsonPropertyName("iconColor")] public string IconColor { get; set; } = "";
}

public class ArchitectureData
{
    [JsonPropertyName("layers")] public List<ArchLayer> Layers { get; set; } = new();
    [JsonPropertyName("techStack")] public List<string> TechStack { get; set; } = new();
}

public class ArchLayer
{
    [JsonPropertyName("number")] public int Number { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("description")] public string Description { get; set; } = "";
}

public class GitHubRepo
{
    [JsonPropertyName("stargazers_count")] public int StargazersCount { get; set; }
    [JsonPropertyName("forks_count")] public int ForksCount { get; set; }
}

public class GitHubRelease
{
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("tag_name")] public string? TagName { get; set; }
    [JsonPropertyName("body")] public string? Body { get; set; }
    [JsonPropertyName("html_url")] public string? HtmlUrl { get; set; }
    [JsonPropertyName("published_at")] public DateTime? PublishedAt { get; set; }
    [JsonPropertyName("assets")] public List<GitHubAsset>? Assets { get; set; }
}

public class GitHubAsset
{
    [JsonPropertyName("browser_download_url")] public string? BrowserDownloadUrl { get; set; }
    [JsonPropertyName("download_count")] public int DownloadCount { get; set; }
    [JsonPropertyName("size")] public long Size { get; set; }
}
