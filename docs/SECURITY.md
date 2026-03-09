# Security Policy

## Supported Versions

| Version | Supported |
|---|---|
| 1.0.x | ✅ Active |

## Reporting a Vulnerability

If you discover a security vulnerability in SharpIB, please report it responsibly.

### How to Report

1. **Do NOT open a public GitHub issue** for security vulnerabilities
2. Instead, please email the maintainer directly or use GitHub's private vulnerability reporting feature
3. Include:
   - A description of the vulnerability
   - Steps to reproduce
   - Potential impact
   - Suggested fix (if any)

### What to Expect

- Acknowledgment within 48 hours
- A fix timeline within 7 days for critical issues
- Credit in the changelog and release notes (unless you prefer to remain anonymous)

## Scope

Since SharpIB runs entirely locally with no network access, the primary security concerns are:

- **Local data exposure** — The SQLite database contains app usage history
- **Win32 API misuse** — The P/Invoke calls should only read window titles and process names
- **Dependency vulnerabilities** — Third-party NuGet packages

## Data Privacy

SharpIB does not:
- Make any network requests
- Send telemetry or analytics
- Access file contents, keystrokes, or clipboard data
- Require elevated permissions
