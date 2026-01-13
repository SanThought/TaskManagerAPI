# --- build ---

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["TaskMasterAPI.csproj", "./"]
RUN dotnet restore "./TaskMasterAPI.csproj"

COPY . .
RUN dotnet publish "./TaskMasterAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- runtime ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Bind to all interfaces inside container
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

# Ensure SQLite folder exists (will be a mounted volume in compose)
RUN mkdir -p /app/App_Data

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TaskMasterAPI.dll"]
