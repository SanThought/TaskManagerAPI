using Microsoft.EntityFrameworkCore;
using TaskMasterAPI.Models;

namespace TaskMasterAPI.Data;

public sealed class TaskDbContext : DbContext
{
    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var tasks = modelBuilder.Entity<TaskItem>();

        tasks.ToTable("Tasks");
        tasks.HasKey(t => t.Id);

        tasks.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(120);

        tasks.Property(t => t.IsCompleted).IsRequired();
        tasks.Property(t => t.CreatedAtUtc).IsRequired();

        tasks.HasIndex(t => t.CreatedAtUtc);
    }
}

