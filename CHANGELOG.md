# Changelog

## v0.3
- Dockerized API (multi-stage Dockerfile)
- Docker Compose runs API on port 8080 with Swagger enabled (Development)
- SQLite file persisted via named volume mounted to /app/App_Data
- App applies EF Core migrations automatically on startup
- Added xUnit integration tests (WebApplicationFactory + SQLite in-memory)
- Added GitHub Actions CI (restore/build/test)
