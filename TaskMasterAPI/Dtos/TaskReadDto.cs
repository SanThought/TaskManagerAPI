namespace TaskMasterAPI.Dtos;

public sealed record TaskReadDto(int Id, string Title, bool IsCompleted, DateTime CreatedAtUtc);

