# Architecture

SharpIB is built using **Clean Architecture** with strict separation of concerns. Each layer has a defined responsibility and can only depend on layers below it.

## Layer Diagram

```
┌──────────────────────────────────────┐
│            SharpIB.UI (WPF)            │  ← Presentation
│  Pages, ViewModels, XAML, DI Setup   │
├──────────────────────────────────────┤
│         SharpIB.Infrastructure         │  ← Data Access
│  EF Core, SQLite, Win32 P/Invoke     │
├──────────────────────────────────────┤
│          SharpIB.Application           │  ← Business Logic
│  CQRS Commands/Queries, DTOs        │
├──────────────────────────────────────┤
│            SharpIB.Domain              │  ← Core
│  Entities, Enums, Interfaces         │
└──────────────────────────────────────┘
```

## Dependency Rules

| Layer | Can depend on |
|---|---|
| Domain | Nothing (zero dependencies) |
| Application | Domain only |
| Infrastructure | Domain + Application |
| UI | Application + Infrastructure |

## Domain Layer

The innermost layer. Contains the core business model with no external dependencies.

### Entities

| Entity | Purpose |
|---|---|
| `ActivityRecord` | A single usage session — process name, window title, start/end time, category |
| `Goal` | A productivity goal — target/limit type, duration, tracked processes, streak |
| `Category` | App classification bucket — Productive, Neutral, or Distracting |
| `AppCategoryMapping` | Maps a process name to a category for auto-classification |
| `DailySnapshot` | Pre-computed daily summary for fast dashboard rendering |

### Enums

| Enum | Values |
|---|---|
| `ProductivityLevel` | Productive, Neutral, Distracting |
| `GoalType` | Target (reach X hours), Limit (stay under X hours) |
| `GoalStatus` | Active, Paused, Completed, Archived |
| `TrackingState` | Running, Paused, Idle |

### Repository Interfaces

- `IActivityRepository` — CRUD for activity records, query by date range
- `IGoalRepository` — CRUD for goals, get active goals
- `IDailySnapshotRepository` — Upsert and query daily snapshots
- `ICategoryRepository` — CRUD for categories and app-to-category mappings

## Application Layer

Contains use cases implemented as CQRS commands and queries via MediatR.

### Commands (Write Operations)

| Command | What It Does |
|---|---|
| `RecordActivityCommand` | Saves a new activity record, auto-assigns category via mapping |
| `CreateGoalCommand` | Creates a new productivity goal |
| `DeleteGoalCommand` | Removes a goal by ID |
| `AssignCategoryCommand` | Maps a process name to a category |

### Queries (Read Operations)

| Query | What It Returns |
|---|---|
| `GetTodayActivitiesQuery` | All activity records for today |
| `GetTopAppsQuery` | Top N apps by total duration in a date range |
| `GetGoalProgressQuery` | All active goals with current progress percentage |
| `GetHourlyBreakdownQuery` | Usage duration for each hour of a given day |
| `GetAllAppsSummaryQuery` | All apps with aggregated stats for a date range |

### Service Interfaces

- `IActiveWindowService` — Abstraction for Win32 foreground window detection

## Infrastructure Layer

Implements persistence and platform-specific services.

### Database

- **ORM:** Entity Framework Core
- **Provider:** SQLite (file-based, zero-config)
- **Location:** `%LOCALAPPDATA%/SharpIB/SharpIB.db`
- **Schema:** Auto-created via `EnsureCreated()` on startup

### Win32 Active Window Service

The `ActiveWindowService` uses P/Invoke to call three Win32 APIs:

| API | Purpose |
|---|---|
| `GetForegroundWindow()` | Gets the handle of the currently focused window |
| `GetWindowText()` | Reads the window title from a handle |
| `GetWindowThreadProcessId()` | Gets the process ID owning the window |

The process ID is then used with `Process.GetProcessById()` to get the process name and executable path.

## UI Layer

WPF presentation layer with Material Design dark theme.

### MVVM Pattern

All pages use the **Model-View-ViewModel** pattern via CommunityToolkit.Mvvm:

| Component | Technology |
|---|---|
| Views | XAML UserControls (pages) |
| ViewModels | `ObservableObject` with `[ObservableProperty]` and `[RelayCommand]` |
| Data Binding | WPF two-way binding |
| Navigation | ViewModel-driven via `ContentControl` + `DataTemplate` |

### Pages

| Page | ViewModel | Purpose |
|---|---|---|
| Dashboard | `DashboardViewModel` | Today's stats, charts, goal progress |
| Applications | `ApplicationsViewModel` | App list, search, category assignment |
| Goals | `GoalsViewModel` | Create/delete goals, progress tracking |
| Reports | `ReportsViewModel` | Date-range analytics, trend charts |

### Dependency Injection

The UI layer configures the entire DI container in `App.xaml.cs` using `Microsoft.Extensions.DependencyInjection`:

- `DbContext` → Scoped
- Repositories → Scoped
- `ActiveWindowService` → Singleton
- ViewModels → Transient (except `MainViewModel` → Singleton)
- MediatR → Auto-registered from Application assembly
