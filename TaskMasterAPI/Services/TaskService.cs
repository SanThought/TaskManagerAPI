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

    public IEnumerable<TaskReadDto> GetAll()
        => _repo.GetAll().Select(ToReadDto);

    public TaskReadDto? GetById(int id)
    {
        var item = _repo.GetById(id);
        return item is null ? null : ToReadDto(item);
    }

    public TaskReadDto Create(TaskCreateDto dto)
    {
        var item = new TaskItem
        {
            Title = dto.Title.Trim(),
            IsCompleted = false,
            CreatedAtUtc = DateTime.UtcNow
        };

        var created = _repo.Add(item);
        return ToReadDto(created);
    }

    public bool Update(int id, TaskUpdateDto dto)
    {
        var existing = _repo.GetById(id);
        if (existing is null) return false;

        var updated = new TaskItem
        {
            Id = existing.Id,
            Title = dto.Title.Trim(),
            IsCompleted = dto.IsCompleted,
            CreatedAtUtc = existing.CreatedAtUtc
        };

        return _repo.Update(updated);
    }

    public bool Delete(int id)
        => _repo.Delete(id);

    private static TaskReadDto ToReadDto(TaskItem item)
        => new(item.Id, item.Title, item.IsCompleted, item.CreatedAtUtc);
}

