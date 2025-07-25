# ClinicFlow

ClinicFlow is an example solution structured around **Clean Architecture** principles. The repository contains several projects that represent separate layers:

- **Domain** – core business models and logic
- **Application** – CQRS handlers and interfaces for use cases
- **Infrastructure** – implementation details (e.g., database, external services)
- **API** – ASP.NET Core entry point exposing HTTP endpoints


The project targets **.NET 8** (see `Directory.Build.props`). Command and Query responsibilities will be separated using a Mediator pattern, and authentication will rely on **JWT** tokens.

## .NET 8 SDK

This repository requires the **.NET 8 SDK**. Install the SDK before building or running the solution. The cross-platform install script provided by Microsoft is the quickest way to get started:

```bash
curl -fsSL https://dot.net/install.sh | bash /dev/stdin --channel 8.0
```

On Windows you can install via `winget`:

```powershell
winget install Microsoft.DotNet.SDK.8
```

For other installation options, see the [official documentation](https://learn.microsoft.com/dotnet/core/install/).

## Building

1. Ensure the .NET 8 SDK is installed (see the section above).
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

Once running, navigate to `http://localhost:5000/swagger` to explore the API using Swagger UI.

The application listens on the URLs defined in `appsettings.json` and `launchSettings.json`.

## Database

Entity Framework Core migrations are included in the Infrastructure project. To apply them to the configured database run:

```bash
cd ClinicFlow
dotnet ef database update --project ClinicFlow.Infrastructure/ClinicFlow.Infrastructure.csproj \
    --startup-project ClinicFlow.API/ClinicFlow.API.csproj
```

## Usage

1. **Request a token**
   
   Send a `POST` request to `/token` with JSON credentials:

   ```bash
   curl -X POST http://localhost:5000/token \
     -H "Content-Type: application/json" \
     -d '{ "username": "admin", "password": "password" }'
   ```

   The response contains a JWT token in the form:

   ```json
   { "token": "<your_token_here>" }
   ```

2. **Call protected routes**

   Include the returned token in the `Authorization` header:

   ```bash
   curl http://localhost:5000/protected \
     -H "Authorization: Bearer <your_token_here>"
   ```

3. **Example endpoints**
   
   - `/greeting` – returns a greeting from a Mediator handler
   - `/public` – accessible without authentication
   - `/protected` – requires a valid token
   - `/admin` – accessible only to users with the `Admin` role

JWT settings (issuer, audience, key) can be customized in `ClinicFlow.API/appsettings.json`.

## Running Tests

After installing the SDK you can restore dependencies and execute the unit tests:

```bash
cd ClinicFlow
dotnet restore
dotnet test ClinicFlow.Tests/ClinicFlow.Tests.csproj
```

## Notes

This repository currently contains minimal starter code. Future work includes adding CQRS handlers, JWT authentication middleware, and infrastructure implementations.
