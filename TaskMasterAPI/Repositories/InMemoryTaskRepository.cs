
using System.Collections.Concurrent;
using System.Threading;
using TaskMasterAPI.Models;

namespace TaskMasterAPI.Repositories;

public sealed class InMemoryTaskRepository : ITaskRepository
{
    private readonly ConcurrentDictionary<int, TaskItem> _store = new();
    private int _idCounter = 0;

    public Task<List<TaskItem>> GetAllAsync(CancellationToken ct = default)
        => Task.FromResult(_store.Values.OrderBy(t => t.Id).ToList());

    public Task<TaskItem?> GetByIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_store.TryGetValue(id, out var item) ? item : null);

    public Task<TaskItem> AddAsync(TaskItem item, CancellationToken ct = default)
    {
        var id = Interlocked.Increment(ref _idCounter);

        var toStore = new TaskItem
        {
            Id = id,
            Title = item.Title,
            IsCompleted = item.IsCompleted,
            CreatedAtUtc = item.CreatedAtUtc == default ? DateTime.UtcNow : item.CreatedAtUtc
        };

        _store[id] = toStore;
        return Task.FromResult(toStore);
    }

    public Task<bool> UpdateAsync(TaskItem item, CancellationToken ct = default)
    {
        if (!_store.ContainsKey(item.Id))
            return Task.FromResult(false);

        _store[item.Id] = item;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_store.TryRemove(id, out _));
}

