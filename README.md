# SharpIB â€” Desktop Activity Manager

A premium, enterprise-grade desktop application that **tracks your system usage in real-time, helps you set productivity goals, and generates actionable reports** â€” all running locally with zero cloud dependency.

Built with **C# WPF** and **.NET 10** using Clean Architecture.

## Key Features

- **Real-Time Activity Tracking** â€” Automatically detects which application you are using via Win32 APIs. Tracks process name, window title, and duration for every session.
- **Productivity Goals** â€” Set daily targets ("Code for 4 hours") or limits ("Browser under 2 hours") with progress tracking and streak counters.
- **Live Dashboard** â€” Today's screen time, productive vs. distracting breakdown, top applications bar chart, hourly activity heatmap, and a real-time productivity score.
- **Application Manager** â€” Full list of every tracked application with total time, session count, and user-assignable productivity categories (Productive, Neutral, Distracting).
- **Reports & Analytics** â€” Date-range driven trend line charts, category pie charts, and ranked top applications tables for daily, weekly, and monthly analysis.
- **100% Offline & Private** â€” All data stored locally in SQLite. No network requests. No telemetry. Your data never leaves your machine.

## Solution Architecture

```
SharpIB/
├── SharpIB.slnx                     # Solution file
├── SharpIB.Domain/                   # Core entities, enums, repository interfaces
├── SharpIB.Application/              # CQRS commands/queries, DTOs, service interfaces
├── SharpIB.Infrastructure/           # EF Core + SQLite, Win32 P/Invoke tracking, repositories
├── SharpIB.UI/                       # WPF Desktop presentation layer (Windows Only)
└── Site/                             # Blazor WebAssembly Web UI
```

| Layer | Responsibility |
|---|---|
| **SharpIB.Domain** | Entities (ActivityRecord, Goal, Category, DailySnapshot), Enums, Repository Interfaces. Zero external dependencies. |
| **SharpIB.Application** | Use cases via MediatR CQRS (RecordActivity, CreateGoal, GetTopApps, etc). DTOs and service abstractions. |
| **SharpIB.Infrastructure** | Concrete persistence via EF Core + SQLite. Win32 ActiveWindowService using P/Invoke for foreground window detection. |
| **SharpIB.UI** | WPF shell with sidebar navigation. Dashboard, Applications, Goals, and Reports pages. Material Design dark theme. LiveCharts for data visualization. |

## Tech Stack

| Component | Technology |
|---|---|
| Framework | .NET 10 WPF |
| Architecture | Clean Architecture, CQRS, Repository Pattern |
| ORM | Entity Framework Core + SQLite |
| CQRS | MediatR |
| MVVM | CommunityToolkit.Mvvm |
| UI Theme | Material Design In XAML (Dark, DeepPurple/Teal) |
| Charts | LiveChartsCore.SkiaSharpView.WPF |
| Tracking | Win32 P/Invoke (GetForegroundWindow, GetWindowText) |

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Windows 10 or later (WPF is Windows-specific)

## Build and Run

```bash
# Clone and navigate to the project
cd c-sharp

# Restore dependencies
dotnet restore SharpIB.slnx

# Build the entire solution (Desktop + Site)
dotnet build SharpIB.slnx

# Run the Desktop Application
dotnet run --project SharpIB.UI/SharpIB.UI.csproj

# Run the Site (Blazor WebAssembly) locally
dotnet run --project Site/site.csproj
```

## Publishing Releases

To create standalone, optimized builds for distribution:

```bash
# Publish the Windows Desktop Application (Self-Contained)
dotnet publish SharpIB.UI/SharpIB.UI.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o release/desktop

# Publish the Site
dotnet publish Site/site.csproj -c Release -o release/site
```

The application will automatically create its SQLite database at `%LOCALAPPDATA%/SharpIB/SharpIB.db` on first launch.

## How It Works

1. **Tracking** â€” A background timer polls the active foreground window every 3 seconds using Win32 APIs. When the user switches applications, the previous session is recorded to the database with start/end timestamps.
2. **Categorization** â€” Users assign applications to categories (Productive, Neutral, Distracting). New activity records automatically inherit their category via process-name mapping.
3. **Goals** â€” Users create daily goals of type Target ("reach X hours") or Limit ("stay under X hours"). The goal engine evaluates progress in real-time and maintains streak counters.
4. **Reporting** â€” Queries aggregate activity records by date range, producing trend lines, category breakdowns, and ranked application tables.

## Data Privacy

All data is stored locally in a SQLite database. The application makes **zero network requests**. Only the following information is captured:

- Process name (e.g., `devenv`)
- Window title (e.g., `MainWindow.xaml â€” SharpIB`)
- Executable path
- Start and end timestamps

No keystrokes, screenshots, or file contents are captured.

## Developer Setup

1. Open `SharpIB.slnx` using **Visual Studio 2022**, **JetBrains Rider**, or **VS Code** (with C# Dev Kit).
2. Set `SharpIB.UI` as the Startup Project.
3. Press `F5` to build, attach the debugger, and run.

