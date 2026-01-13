using TaskMasterAPI.Dtos;
using TaskMasterAPI.Models;
using TaskMasterAPI.Repositories;

namespace TaskMasterAPI.Services;

public sealed class TaskService : ITaskService
{
    private readonly ITaskRepository _repo;

    public TaskService(ITaskRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<TaskReadDto>> GetAllAsync(CancellationToken ct = default)
        => (await _repo.GetAllAsync(ct)).Select(ToReadDto);

    public async Task<TaskReadDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var item = await _repo.GetByIdAsync(id, ct);
        return item is null ? null : ToReadDto(item);
    }

    public async Task<TaskReadDto> CreateAsync(TaskCreateDto dto, CancellationToken ct = default)
    {
	var item = new TaskItem
        {
            Title = dto.Title.Trim(),
            IsCompleted = false,
            CreatedAtUtc = DateTime.UtcNow
        };

        var created = await _repo.AddAsync(item, ct);
        return ToReadDto(created);
    }

    public async Task<bool> UpdateAsync(int id, TaskUpdateDto dto, CancellationToken ct = default)
    {
        // preserve CreatedAtUtc
        var existing = await _repo.GetByIdAsync(id, ct);
        if (existing is null) return false;

        var updated = new TaskItem
        {
            Id = existing.Id,
            Title = dto.Title.Trim(),
            IsCompleted = dto.IsCompleted,
            CreatedAtUtc = existing.CreatedAtUtc
        };

        return await _repo.UpdateAsync(updated, ct);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        => _repo.DeleteAsync(id, ct);

    private static TaskReadDto ToReadDto(TaskItem item)
        => new(item.Id, item.Title, item.IsCompleted, item.CreatedAtUtc);
}

