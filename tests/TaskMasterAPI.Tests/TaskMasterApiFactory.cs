using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskMasterAPI.Data;

namespace TaskMasterAPI.Tests;

public sealed class TaskMasterApiFactory : WebApplicationFactory<Program>
{
    private SqliteConnection? _connection;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            // Remove the app's TaskDbContext registration so tests don't touch local/dev SQLite files.
            var dbContextOptionsDescriptors = services
                .Where(d => d.ServiceType == typeof(DbContextOptions<TaskDbContext>))
                .ToList();

            foreach (var d in dbContextOptionsDescriptors)
                services.Remove(d);

            // SQLite in-memory (keep connection open for lifetime of factory)
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            services.AddDbContext<TaskDbContext>(options =>
                options.UseSqlite(_connection));
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _connection?.Dispose();
            _connection = null;
        }
    }
}

