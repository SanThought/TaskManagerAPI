using TaskMasterAPI.Dtos;

namespace TaskMasterAPI.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskReadDto>> GetAllAsync(CancellationToken ct = default);
    Task<TaskReadDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<TaskReadDto> CreateAsync(TaskCreateDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, TaskUpdateDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}

