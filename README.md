# ClawSome

Summary
-------
ClawSome is a small .NET 9 ASP.NET Web API that returns a simple profile payload (user info + a random cat fact). The project reads user info from configuration and calls an external Cat Facts API.

Repository
----------
https://github.com/BugHashira/ClawSome

Prerequisites
-------------
- .NET 9 SDK — install from https://dotnet.microsoft.com/ (required)
- Git (optional, for cloning)
- Docker (optional, to build/run container)
- Visual Studio 2022 (latest updates) or any code editor (VS Code, Rider)

Dependencies
------------
These NuGet packages are referenced in ClawSome.csproj and will be restored by dotnet restore:
- Microsoft.AspNetCore.OpenApi (v9.0.9) — minimal OpenAPI support
- Microsoft.VisualStudio.Azure.Containers.Tools.Targets — tooling support for Docker in VS

How to install dependencies
---------------------------
From the repository root:
1. Restore packages:
   dotnet restore

The dotnet CLI and Visual Studio restore referenced packages automatically.

Run locally (CLI)
-----------------
1. Restore and build:
   dotnet restore
   dotnet build

2. Run:
   dotnet run --project ClawSome.csproj

3. The server will print the listening URL(s) to the console. Then request:
   GET {URL}/me
   Example:
   curl http://localhost:5000/me

Run locally (Visual Studio)
---------------------------
1. Open the solution/project in Visual Studio 2022.
2. Set the project as the startup project.
3. Use __Debug > Start Debugging__ (F5) or __Debug > Start Without Debugging__ (Ctrl+F5).
4. Browse to /me on the printed URL.

Run with Docker
---------------
Build the image:
docker build -t clawsome:latest .

Run the container (set PORT used by hosting in Dockerfile):
docker run -e PORT=8080 -p 8080:8080 clawsome:latest

Then request:
curl http://localhost:8080/me

Configuration / Environment variables
-------------------------------------
Configuration is read from appsettings.json (and environment overrides supported via ASP.NET Core configuration rules).

Important configuration keys:
- User:Name, User:Email, User:Stack
  - These come from appsettings.json by default. You can override with environment variables using double-underscore notation:
    - User__Name
    - User__Email
    - User__Stack

- ExternalApis:CatFactApiUrl
  - Default: https://catfact.ninja/fact
  - Override via environment variable: ExternalApis__CatFactApiUrl

- ExternalApis:TimeoutInSeconds
  - Default: 5
  - Override via environment variable: ExternalApis__TimeoutInSeconds

- ASPNETCORE_URLS
  - You can set ASPNETCORE_URLS to control Kestrel listening endpoint(s).
  - When running via Dockerfile, the Dockerfile expects a PORT environment variable and sets ASPNETCORE_URLS to http://+:${PORT} (so pass PORT when running container).

Examples
--------
Override user name and run:
User__Name="Alice" dotnet run --project ClawSome.csproj

Call the API:
curl http://localhost:5000/me

Notes about the codebase
------------------------
- Program.cs wires services, config, and CORS.
- Services/CatFactService.cs uses IHttpClientFactory and configuration for external API and timeout.
- Controller at /me returns a ProfileDto containing configured user and the cat fact.
- No database or secret store is required for current functionality.
- No unit tests are included in this repository.
