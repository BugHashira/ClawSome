# Use .NET 9 SDK (Linux) for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["ClawSome.csproj", "./"]
RUN dotnet restore "ClawSome.csproj"

# Copy all source files and build
COPY . .
RUN dotnet publish "ClawSome.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use .NET 9 ASP.NET runtime for final image (Linux)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# Set the environment variable for Railway port
ENV ASPNETCORE_URLS=http://+:${PORT}

# Expose the port Railway assigns
EXPOSE ${PORT}

# Run the application
ENTRYPOINT ["dotnet", "ClawSome.dll"]
