# Development Guide

## Project Structure

```
c-sharp/
├── .github/                      # GitHub templates
│   ├── ISSUE_TEMPLATE/
│   │   ├── bug_report.md         # Bug report template
│   │   └── feature_request.md    # Feature request template
│   ├── CONTRIBUTING.md           # Contributing guidelines
│   └── PULL_REQUEST_TEMPLATE.md  # PR template
├── docs/                         # Documentation and GitHub Pages site
│   ├── index.html                # Landing page (GitHub Pages)
│   ├── README.md                 # Docs index
│   ├── getting-started.md        # Installation guide
│   ├── architecture.md           # Architecture overview
│   ├── features.md               # Feature documentation
│   └── development.md            # This file
├── legacy/                       # Legacy code (archived)
├── SharpIB.Domain/                 # Core entities and interfaces
│   ├── Entities/
│   │   ├── ActivityRecord.cs     # Usage session entity
│   │   ├── Category.cs           # Category + AppCategoryMapping
│   │   ├── DailySnapshot.cs      # Daily summary entity
│   │   └── Goal.cs               # Productivity goal entity
│   ├── Enums/
│   │   └── Enums.cs              # ProductivityLevel, GoalType, GoalStatus, TrackingState
│   └── Interfaces/
│       └── IRepositories.cs      # Repository contracts
├── SharpIB.Application/            # CQRS commands, queries, DTOs
│   ├── Commands/
│   │   └── Commands.cs           # RecordActivity, CreateGoal, DeleteGoal, AssignCategory
│   ├── Queries/
│   │   └── Queries.cs            # GetTodayActivities, GetTopApps, GetGoalProgress, etc.
│   ├── DTOs/
│   │   └── DTOs.cs               # ActivityDto, AppUsageSummaryDto, GoalProgressDto, etc.
│   └── Services/
│       └── IActiveWindowService.cs
├── SharpIB.Infrastructure/         # EF Core, repositories, Win32 tracking
│   ├── Data/
│   │   └── SharpIBDbContext.cs     # DbContext with entity configs and seed data
│   ├── Repositories/
│   │   └── Repositories.cs       # All repository implementations
│   └── Services/
│       └── ActiveWindowService.cs # Win32 P/Invoke window tracker
├── SharpIB.UI/                     # WPF presentation layer
│   ├── App.xaml                  # Material Design theme configuration
│   ├── App.xaml.cs               # DI container setup and database initialization
│   ├── AssemblyInfo.cs           # WPF ThemeInfo
│   ├── MainWindow.xaml           # Shell with sidebar navigation
│   ├── MainWindow.xaml.cs        # MainWindow code-behind
│   ├── ViewModels/
│   │   ├── MainViewModel.cs      # Navigation, tracking timer, app switch detection
│   │   ├── DashboardViewModel.cs # Dashboard stats, charts, goal progress
│   │   ├── ApplicationsViewModel.cs # App list, search, category assignment
│   │   ├── GoalsViewModel.cs     # Goal CRUD, progress tracking
│   │   └── ReportsViewModel.cs   # Date-range analytics, trend charts
│   └── Pages/
│       ├── DashboardPage.xaml    # Dashboard UI
│       ├── ApplicationsPage.xaml # Applications UI
│       ├── GoalsPage.xaml        # Goals UI
│       └── ReportsPage.xaml      # Reports UI
├── .gitignore
├── SharpIB.slnx                    # Solution file
└── README.md                     # Project overview
```

## Coding Conventions

### C# Style

- **Primary constructors** for dependency injection
- **File-scoped namespaces** (`namespace X;`)
- **Records** for immutable DTOs
- **`[ObservableProperty]`** and **`[RelayCommand]`** source generators from CommunityToolkit.Mvvm
- **Nullable** enabled globally

### XAML Style

- Material Design components and icons throughout
- Global styles defined in `App.xaml` (`CardStyle`, `PageTitle`)
- Dark theme with DeepPurple primary and Teal secondary accents
- DataTriggers for conditional styling

### Architecture Rules

1. **Domain** must have zero NuGet dependencies
2. **Application** depends only on Domain
3. **Infrastructure** depends on Domain and Application
4. **UI** depends on Application and Infrastructure (for DI wiring only)
5. All cross-layer communication uses interfaces defined in Domain or Application

## Adding a New Feature

### Adding a New Entity

1. Create the entity class in `SharpIB.Domain/Entities/`
2. Add a `DbSet<>` in `SharpIBDbContext` and configure it in `OnModelCreating`
3. Define a repository interface in `SharpIB.Domain/Interfaces/`
4. Implement the repository in `SharpIB.Infrastructure/Repositories/`
5. Register it in `App.xaml.cs` DI container

### Adding a New CQRS Command/Query

1. Create the record class implementing `IRequest<T>` in `SharpIB.Application/Commands/` or `Queries/`
2. Create the handler class implementing `IRequestHandler<TRequest, TResponse>`
3. MediatR will auto-discover it — no registration needed

### Adding a New Page

1. Create a ViewModel in `SharpIB.UI/ViewModels/` extending `ObservableObject`
2. Create a XAML `UserControl` in `SharpIB.UI/Pages/`
3. Add a `DataTemplate` in `MainWindow.xaml` mapping the ViewModel to the page
4. Register the ViewModel in `App.xaml.cs` DI container
5. Add navigation in `MainViewModel.NavigateTo()`

## Build Commands

```bash
# Restore packages
dotnet restore SharpIB.slnx

# Build all projects
dotnet build SharpIB.slnx

# Run the application
dotnet run --project SharpIB.UI/SharpIB.UI.csproj

# Clean build artifacts
dotnet clean SharpIB.slnx

# Publish a release build of the Windows Desktop App
dotnet publish SharpIB.UI/SharpIB.UI.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o release/desktop

# Publish a release build of the Web Site
dotnet publish Site/site.csproj -c Release -o release/site
```
