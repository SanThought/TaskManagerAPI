## Docker (v0.3)

Run:
```bash
docker compose up --build
````

Swagger:

* [http://localhost:8080/swagger](http://localhost:8080/swagger)

DB persistence:

* Compose mounts a named volume to `/app/App_Data` inside the container so SQLite persists across restarts.

Stop (keep data):

```bash
docker compose down
```

Stop + delete volume (wipe DB):

```bash
docker compose down -v
```

## Migrations

Local apply:

```bash
dotnet ef database update
```

In v0.3, migrations are also applied automatically on app startup (`Database.Migrate()`), including in Docker.

## Tests

Run locally:

```bash
dotnet test
```

Integration tests use `WebApplicationFactory<Program>` and override EF Core to SQLite in-memory to be deterministic in CI.

## CI

GitHub Actions runs on push/PR:

* dotnet restore
* dotnet build
* dotnet test
