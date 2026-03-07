# Features

## Real-Time Activity Tracking

Pulse automatically monitors which application you are using at any given time.

**How it works:**

1. A background timer fires every **3 seconds**
2. The `ActiveWindowService` calls Win32 `GetForegroundWindow()` to identify the active window
3. When a window switch is detected (different process or title), the previous session is saved as an `ActivityRecord`
4. Sessions shorter than 2 seconds are filtered out as noise

**What is captured:**

| Data Point | Example |
|---|---|
| Process Name | `devenv`, `chrome`, `code` |
| Window Title | `MainWindow.xaml — Pulse` |
| Executable Path | `C:\Program Files\...\devenv.exe` |
| Start Time | `2026-03-07 14:32:15` |
| End Time | `2026-03-07 14:45:08` |

**What is NOT captured:** Keystrokes, screenshots, clipboard content, file contents, or network traffic.

---

## Dashboard

The Dashboard provides a real-time overview of your day.

### Stat Cards

| Card | Description |
|---|---|
| Screen Time | Total active time across all applications today |
| Productive | Time spent in apps categorized as Productive |
| Distracting | Time spent in apps categorized as Distracting |
| Score | Productivity score (productive time / total time × 100) |

### Top Applications Chart

A bar chart showing the top 6 most-used applications today, ranked by total duration in minutes.

### Hourly Activity Heatmap

A 24-column bar chart showing how much time was spent actively using the computer during each hour of the day. Useful for identifying peak productivity hours and break patterns.

### Goal Progress Cards

Displays all active goals with:
- Progress bar (0–100%)
- Current vs. target duration
- Completion status (checkmark or cross)
- Current streak count

---

## Application Manager

A comprehensive list of every application Pulse has tracked.

### Features

- **Search** — Filter applications by process name
- **Duration** — Total time spent in each application
- **Session Count** — Number of individual sessions recorded
- **Category Badge** — Shows the assigned productivity category with color coding
- **Assign Button** — Click to cycle through categories (Productive → Neutral → Distracting)

### Category System

| Category | Color | Meaning |
|---|---|---|
| Productive | Green | Apps that contribute to your work (IDEs, design tools, etc.) |
| Neutral | Grey | Apps that are neither productive nor distracting (file manager, settings) |
| Distracting | Red | Apps that reduce productivity (social media, games, etc.) |

Categories are assigned per process name. Once assigned, all future activity records for that process automatically inherit the category.

---

## Goals

Set measurable daily productivity goals and track your streaks.

### Goal Types

| Type | Description | Example |
|---|---|---|
| **Target** | Reach at least X hours per day | "Code for 4 hours" |
| **Limit** | Stay under X hours per day | "Browser under 2 hours" |

### Creating a Goal

1. Click **New Goal** on the Goals page
2. Fill in the title, optional description, and tracked processes (comma-separated)
3. Select the type (Target or Limit)
4. Set the target hours and minutes
5. Click **Create Goal**

### Tracked Processes

Goals track specific applications by process name. For example, to track coding time you might enter: `devenv,code,rider64`

If no processes are specified, the goal tracks all activity.

### Streaks

The streak counter tracks consecutive days of meeting your goal. A streak of 7 means you've hit your target for 7 days in a row.

---

## Reports

Analyze your usage patterns over any date range.

### Summary Cards

| Metric | Description |
|---|---|
| Total Time | Sum of all active time in the selected range |
| Avg / Day | Average daily active time |
| Avg Score | Average daily productivity score |

### Usage Trend

A line chart showing total active hours per day across the selected date range. Useful for identifying trends — are you becoming more or less productive over time?

### Category Breakdown

A pie chart showing the proportion of time spent in Productive, Neutral, and Distracting applications.

### Top Applications Table

A ranked table of all applications used in the selected period, showing:
- Rank
- Process name
- Total duration
- Session count
- Category assignment

---

## Privacy and Data

### Where Data Is Stored

All data is stored in a local SQLite database:

```
%LOCALAPPDATA%\Pulse\pulse.db
```

### Network Activity

Pulse makes **zero network requests**. There is no telemetry, no analytics, no crash reporting, and no update checking.

### Data You Control

You can delete the entire database at any time by removing the `pulse.db` file. The application will recreate an empty database on next launch.
