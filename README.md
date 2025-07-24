# ClinicFlow

ClinicFlow is an example solution structured around **Clean Architecture** principles. The repository contains several projects that represent separate layers:

- **Domain** – core business models and logic
- **Application** – CQRS handlers and interfaces for use cases
- **Infrastructure** – implementation details (e.g., database, external services)
- **API** – ASP.NET Core entry point exposing HTTP endpoints

The project targets **.NET 8** (see `Directory.Build.props`). Command and Query responsibilities will be separated using a Mediator pattern, and authentication will rely on **JWT** tokens.

## Building

1. Ensure you have [.NET 8 SDK](https://dotnet.microsoft.com/download) installed.
2. Restore and build the solution:

```bash
cd ClinicFlow
dotnet build ClinicFlow.sln
```

## Running

Run the web API project directly:

```bash
cd ClinicFlow
dotnet run --project ClinicFlow.API
```

The application listens on the URLs defined in `appsettings.json` and `launchSettings.json`.

## Notes

This repository currently contains minimal starter code. Future work includes adding CQRS handlers, JWT authentication middleware, and infrastructure implementations.
