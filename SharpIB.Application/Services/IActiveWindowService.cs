namespace SharpIB.Application.Services;

/// <summary>
/// Abstraction for the Win32 active-window tracking service.
/// </summary>
public interface IActiveWindowService
{
    /// <summary>Gets the process name of the currently focused window (e.g., "devenv").</summary>
    string GetActiveProcessName();

    /// <summary>Gets the title of the currently focused window.</summary>
    string GetActiveWindowTitle();

    /// <summary>Gets the full executable path of the active window's process.</summary>
    string GetActiveExecutablePath();
}

