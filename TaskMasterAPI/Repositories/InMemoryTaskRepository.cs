using System.Collections.Concurrent;
using System.Threading;
using TaskMasterAPI.Models;

namespace TaskMasterAPI.Repositories;

public sealed class InMemoryTaskRepository : ITaskRepository
{
    private readonly ConcurrentDictionary<int, TaskItem> _store = new();
    private int _idCounter = 0;

    public IEnumerable<TaskItem> GetAll()
        => _store.Values.OrderBy(t => t.Id);

    public TaskItem? GetById(int id)
        => _store.TryGetValue(id, out var item) ? item : null;

    public TaskItem Add(TaskItem item)
    {
        var id = Interlocked.Increment(ref _idCounter);

        // Ensure we store a new instance (avoid external references mutating our store)
        var toStore = new TaskItem
        {
            Id = id,
            Title = item.Title,
            IsCompleted = item.IsCompleted,
            CreatedAtUtc = item.CreatedAtUtc == default ? DateTime.UtcNow : item.CreatedAtUtc
        };

        _store[id] = toStore;
        return toStore;
    }

    public bool Update(TaskItem item)
    {
        if (!_store.ContainsKey(item.Id))
            return false;

        // Replace atomically
        _store[item.Id] = item;
        return true;
    }

    public bool Delete(int id)
        => _store.TryRemove(id, out _);
}

