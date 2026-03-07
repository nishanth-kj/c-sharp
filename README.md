# Pulse — Desktop Activity Manager

A premium, enterprise-grade desktop application that **tracks your system usage in real-time, helps you set productivity goals, and generates actionable reports** — all running locally with zero cloud dependency.

Built with **C# WPF** and **.NET 10** using Clean Architecture.

## Key Features

- **Real-Time Activity Tracking** — Automatically detects which application you are using via Win32 APIs. Tracks process name, window title, and duration for every session.
- **Productivity Goals** — Set daily targets ("Code for 4 hours") or limits ("Browser under 2 hours") with progress tracking and streak counters.
- **Live Dashboard** — Today's screen time, productive vs. distracting breakdown, top applications bar chart, hourly activity heatmap, and a real-time productivity score.
- **Application Manager** — Full list of every tracked application with total time, session count, and user-assignable productivity categories (Productive, Neutral, Distracting).
- **Reports & Analytics** — Date-range driven trend line charts, category pie charts, and ranked top applications tables for daily, weekly, and monthly analysis.
- **100% Offline & Private** — All data stored locally in SQLite. No network requests. No telemetry. Your data never leaves your machine.

## Solution Architecture

```
Pulse/
├── Pulse.slnx                     # Solution file
├── Pulse.Domain/                   # Core entities, enums, repository interfaces
├── Pulse.Application/              # CQRS commands/queries, DTOs, service interfaces
├── Pulse.Infrastructure/           # EF Core + SQLite, Win32 P/Invoke tracking, repositories
└── Pulse.UI/                       # WPF presentation layer (Material Design, LiveCharts)
```

| Layer | Responsibility |
|---|---|
| **Pulse.Domain** | Entities (ActivityRecord, Goal, Category, DailySnapshot), Enums, Repository Interfaces. Zero external dependencies. |
| **Pulse.Application** | Use cases via MediatR CQRS (RecordActivity, CreateGoal, GetTopApps, etc). DTOs and service abstractions. |
| **Pulse.Infrastructure** | Concrete persistence via EF Core + SQLite. Win32 ActiveWindowService using P/Invoke for foreground window detection. |
| **Pulse.UI** | WPF shell with sidebar navigation. Dashboard, Applications, Goals, and Reports pages. Material Design dark theme. LiveCharts for data visualization. |

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
cd c-sharp/Pulse

# Restore dependencies
dotnet restore Pulse.slnx

# Build the solution
dotnet build Pulse.slnx

# Run the application
dotnet run --project Pulse.UI/Pulse.UI.csproj
```

The application will automatically create its SQLite database at `%LOCALAPPDATA%/Pulse/pulse.db` on first launch.

## How It Works

1. **Tracking** — A background timer polls the active foreground window every 3 seconds using Win32 APIs. When the user switches applications, the previous session is recorded to the database with start/end timestamps.
2. **Categorization** — Users assign applications to categories (Productive, Neutral, Distracting). New activity records automatically inherit their category via process-name mapping.
3. **Goals** — Users create daily goals of type Target ("reach X hours") or Limit ("stay under X hours"). The goal engine evaluates progress in real-time and maintains streak counters.
4. **Reporting** — Queries aggregate activity records by date range, producing trend lines, category breakdowns, and ranked application tables.

## Data Privacy

All data is stored locally in a SQLite database. The application makes **zero network requests**. Only the following information is captured:

- Process name (e.g., `devenv`)
- Window title (e.g., `MainWindow.xaml — Pulse`)
- Executable path
- Start and end timestamps

No keystrokes, screenshots, or file contents are captured.

## Developer Setup

1. Open `Pulse.slnx` using **Visual Studio 2022**, **JetBrains Rider**, or **VS Code** (with C# Dev Kit).
2. Set `Pulse.UI` as the Startup Project.
3. Press `F5` to build, attach the debugger, and run.
