using TaskMasterAPI.Dtos;

namespace TaskMasterAPI.Services;

public interface ITaskService
{
    IEnumerable<TaskReadDto> GetAll();
    TaskReadDto? GetById(int id);
    TaskReadDto Create(TaskCreateDto dto);
    bool Update(int id, TaskUpdateDto dto);
    bool Delete(int id);
}

