using TaskMasterAPI.Models;

namespace TaskMasterAPI.Repositories;

public interface ITaskRepository
{
    Task<List<TaskItem>> GetAllAsync(CancellationToken ct = default);
    Task<TaskItem?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<TaskItem> AddAsync(TaskItem item, CancellationToken ct = default);
    Task<bool> UpdateAsync(TaskItem item, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}

