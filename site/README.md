# Pulse Landing Page

This directory contains the source code for the official Pulse Desktop Activity Manager landing page.

Built with **.NET 10 Blazor WebAssembly Standalone**, this project compiles to a set of pure static files (HTML, CSS, WebAssembly, and JavaScript) that can be hosted on any static file server like GitHub Pages.

## Architecture

The site follows a component-based architecture and relies on JSON files for content management, making it extremely easy to update text and features without touching C# code.

### Key Components
- **`Pages/Home.razor`**: The main composition root. It fetches data dynamically and passes it to child components.
- **`Components/`**: Reusable Blazor components (`Navbar`, `Hero`, `Features`, `Architecture`, `Download`, `CallToAction`, `Footer`).
- **`Utils/Api.cs`**: A reusable HTTP client wrapper used for all external API calls (currently connects to the GitHub API for live release and repo stats).
- **`Models/SiteModels.cs`**: C# typed models mapping to the JSON configurations and GitHub API responses.

### Data-Driven Content (`wwwroot/data/`)
Content is driven by static JSON files loaded at runtime:
- `site-config.json`: Branding, global text, and external URLs.
- `features.json`: Cards for the "Features" section.
- `architecture.json`: Details for the Clean Architecture visualization.

## Configuration

Environment-specific configuration is handled via `.env` (for reference/tooling) and `wwwroot/appsettings.json` (used natively by Blazor).

1. Copy `.env.example` to `.env` if using external tools.
2. In `wwwroot/appsettings.json`, you can define:
```json
{
  "ApiSettings": {
    "BaseUrl": "https://api.github.com",
    "Repo": "nishanth-kj/c-sharp"
  }
}
```

## Local Development

You need the [.NET 10 SDK](https://dotnet.microsoft.com/download) installed.

To run the site locally with hot reload:
```bash
cd site/
dotnet watch run
```

This will launch a local development server (typically at `http://localhost:5000` or `https://localhost:5001`).

## Building & Publishing

To build the site for production (compiling to static files):

```bash
cd site/
dotnet publish -c Release -o ./publish
```

The resulting `publish/wwwroot/` folder will contain everything needed to host the site.

### GitHub Actions Deployment
The repository is configured with a GitHub Actions workflow (`.github/workflows/release.yml`) that automatically builds and deploys this site to GitHub Pages whenever a new version tag (e.g., `v1.0.0`) is pushed. The workflow includes a step to add a `.nojekyll` file, ensuring GitHub Pages correctly serves the `_framework/` directories required by Blazor WASM.
