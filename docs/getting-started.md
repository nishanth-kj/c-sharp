# Getting Started

## Prerequisites

| Requirement | Version |
|---|---|
| .NET SDK | 10.0 or later |
| Operating System | Windows 10 / 11 |
| IDE (optional) | Visual Studio 2022, JetBrains Rider, or VS Code |

## Installation

### From Source

```bash
# 1. Clone the repository
git clone https://github.com/nishanth-kj/c-sharp.git
cd c-sharp

# 2. Restore NuGet packages
dotnet restore Pulse.slnx

# 3. Build the solution
dotnet build Pulse.slnx

# 4. Run the application
dotnet run --project Pulse.UI/Pulse.UI.csproj
```

### First Launch

On first launch, Pulse will:

1. **Create the database** — A SQLite database is automatically created at `%LOCALAPPDATA%/Pulse/pulse.db`
2. **Seed default categories** — Three categories are pre-configured:
   - Productive (green)
   - Neutral (grey)
   - Distracting (red)
3. **Start tracking** — Activity tracking begins immediately

## Configuration

### Database Location

Default: `%LOCALAPPDATA%/Pulse/pulse.db`

This path is set in `Pulse.UI/App.xaml.cs` and can be modified before building.

### Tracking Interval

Default: **3 seconds**

The tracking timer polls the active foreground window every 3 seconds. This interval is configured in `MainViewModel.cs` and can be adjusted:

```csharp
// In MainViewModel constructor
_trackingTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
```

### Minimum Session Duration

Default: **2 seconds**

Activity sessions shorter than 2 seconds are ignored to filter out noise from rapid window switching. Configured in `MainViewModel.FlushCurrentActivityAsync()`.
