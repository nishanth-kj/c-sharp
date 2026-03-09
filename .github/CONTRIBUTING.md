# Contributing to SharpIB

Thank you for your interest in contributing to SharpIB! This document provides guidelines for contributing.

## Getting Started

1. Fork the repository
2. Clone your fork and create a new branch:
   ```bash
   git checkout -b feature/your-feature-name
   ```
3. Make your changes
4. Build and test:
   ```bash
   dotnet build SharpIB.slnx
   dotnet run --project SharpIB.UI/SharpIB.UI.csproj
   ```
5. Commit your changes and push to your fork
6. Open a Pull Request

## Development Setup

- Install the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Windows 10 or later required (WPF)
- Recommended IDE: Visual Studio 2022, JetBrains Rider, or VS Code with C# Dev Kit

## Architecture

SharpIB follows **Clean Architecture** with 4 layers:

| Layer | What to modify |
|---|---|
| `SharpIB.Domain` | Entities, enums, repository interfaces |
| `SharpIB.Application` | CQRS commands/queries, DTOs, service interfaces |
| `SharpIB.Infrastructure` | Database, Win32 tracking, repository implementations |
| `SharpIB.UI` | WPF pages, ViewModels, XAML |

## Guidelines

- Follow existing code patterns and naming conventions
- Use primary constructors for DI
- Keep the Domain layer free of external dependencies
- Write descriptive commit messages
- One feature per pull request

## Reporting Issues

Use the GitHub Issue templates to report bugs or request features.
