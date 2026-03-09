# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Real-time activity tracking via Win32 API (GetForegroundWindow)
- Dashboard with stat cards, top apps chart, hourly activity heatmap
- Application manager with search, category assignment, session tracking
- Productivity goals with Target/Limit types and streak tracking
- Reports page with date-range analytics, trend charts, category pie chart
- Material Design dark theme (DeepPurple/Teal)
- SQLite local database (auto-created at %LOCALAPPDATA%/SharpIB/)
- GitHub Pages landing site with dynamic release downloads
- Full project documentation in docs/
- GitHub templates for issues, PRs, bugs, and feature requests
- Non-commercial open source license

## [1.0.0] — 2026-03-07

### Added
- Initial release
- Complete Clean Architecture (Domain, Application, Infrastructure, UI)
- Activity tracking with 3-second polling interval
- Goal creation and progress monitoring
- Application categorization (Productive, Neutral, Distracting)
- Dashboard, Applications, Goals, and Reports pages
- LiveCharts data visualization
- Win32 P/Invoke active window detection
