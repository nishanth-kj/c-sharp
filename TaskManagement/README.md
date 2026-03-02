# Premium Enterprise Task Manager

A beautifully crafted, high-performance Desktop application built with **C# WPF** and **.NET 8**.

This project demonstrates a production-ready application architecture tailored for scalability, maintainability, and clean code principles. It is built as a portfolio piece to showcase expertise in modern .NET development, clean architecture, and UI/UX design.

## 🚀 Key Features

- **Clean Architecture:** Strict separation of concerns divided into Domain, Application, Infrastructure, and UI layers.
- **CQRS Pattern:** Robust command and query separation handled by MediatR.
- **Local Persistence:** Data is seamlessly managed via **Entity Framework Core** with a local SQLite database, allowing for pure offline capability.
- **Modern UI:** Built using WPF and styled with Material Design Principles, ensuring a stunning and intuitive user experience.
- **Dependency Injection:** Full utilization of the modern .NET Generic Host for DI, making the application heavily decoupled and testable.

## 📁 Solution Architecture

- `TaskManager.Domain`: Contains the core enterprise logic, entities (e.g., `TaskItem`, `Board`), and repository interfaces. Has no external dependencies.
- `TaskManager.Application`: Contains the use cases (CQRS Commands/Queries), validation rules, and DTOs. Depends only on the Domain.
- `TaskManager.Infrastructure`: Implements persistence using EF Core and SQLite. Connects to the database and implements the Domain repository interfaces.
- `TaskManager.UI`: The WPF Desktop application that acts as the presentation layer. Connects everything via Dependency Injection at startup.

## 🛠️ Tech Stack

- **Framework:** .NET 8 WPF
- **Architecture:** Clean Architecture, CQRS, Repository Pattern
- **Libraries:**
  - `MediatR` (CQRS)
  - `FluentValidation` (Data Validation)
  - `Microsoft.EntityFrameworkCore.Sqlite` (ORM & Data Storage)
  - `MaterialDesignThemes` (UI Styling)
  - `CommunityToolkit.Mvvm` (MVVM Framework)

## 🏃‍♂️ How to Run the Application

### Prerequisites
- Install the [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).
- Windows OS (WPF is Windows-specific).

### Build & Run Instructions

1. **Clone the Repository** and navigate to the project root:
   ```bash
   cd c-sharp
   cd TaskManagement
   ```

2. **Restore Dependencies:**
   ```bash
   dotnet restore TaskManager.slnx
   ```

3. **Build the Solution:**
   ```bash
   dotnet build TaskManager.slnx
   ```

4. **Run the Application:**
   ```bash
   dotnet run --project TaskManager.UI/TaskManager.UI.csproj
   ```

*(Note: The application will automatically apply any pending Entity Framework database migrations and create the local SQLite database on its first launch.)*

## 🧑‍💻 Developer Setup

If you wish to open this project in an IDE:
- Open `TaskManager.sln` using **Visual Studio 2022**, **JetBrains Rider**, or **VS Code** (with the C# Dev Kit).
- Set `TaskManager.UI` as the Startup Project.
- Hit `F5` to build, attach the debugger, and run the app.
