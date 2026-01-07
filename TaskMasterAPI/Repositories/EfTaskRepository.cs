using Microsoft.EntityFrameworkCore;
using TaskMasterAPI.Data;
using TaskMasterAPI.Models;

namespace TaskMasterAPI.Repositories;

public sealed class EfTaskRepository : ITaskRepository
{
    private readonly TaskDbContext _db;

    public EfTaskRepository(TaskDbContext db)
    {
        _db = db;
    }

    public async Task<List<TaskItem>> GetAllAsync(CancellationToken ct = default)
        => await _db.Tasks
            .AsNoTracking()
            .OrderBy(t => t.Id)
            .ToListAsync(ct);

    public async Task<TaskItem?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _db.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<TaskItem> AddAsync(TaskItem item, CancellationToken ct = default)
    {
        _db.Tasks.Add(item);
        await _db.SaveChangesAsync(ct);
        return item;
    }

    public async Task<bool> UpdateAsync(TaskItem item, CancellationToken ct = default)
    {
        var exists = await _db.Tasks.AnyAsync(t => t.Id == item.Id, ct);
        if (!exists) return false;

        _db.Tasks.Update(item);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id, ct);
        if (entity is null) return false;

        _db.Tasks.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}

