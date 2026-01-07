using Microsoft.AspNetCore.Mvc;
using TaskMasterAPI.Dtos;
using TaskMasterAPI.Services;

namespace TaskMasterAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TasksController : ControllerBase
{
    private readonly ITaskService _service;

    public TasksController(ITaskService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskReadDto>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskReadDto>> GetById(int id, CancellationToken ct)
    {
        var task = await _service.GetByIdAsync(id, ct);
        if (task is not null) return Ok(task);

        return NotFound(new ProblemDetails
        {
            Title = "Task not found.",
            Status = StatusCodes.Status404NotFound,
            Type = "https://httpstatuses.com/404",
            Detail = $"No task with id {id} was found.",
            Instance = HttpContext.Request.Path
        });
    }

    [HttpPost]
    public async Task<ActionResult<TaskReadDto>> Create([FromBody] TaskCreateDto dto, CancellationToken ct)
    {
        // FluentValidation + [ApiController] => invalid dto returns 400 automatically
        var created = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TaskUpdateDto dto, CancellationToken ct)
        => await _service.UpdateAsync(id, dto, ct) ? NoContent() : NotFound(new ProblemDetails
        {
            Title = "Task not found.",
            Status = StatusCodes.Status404NotFound,
            Type = "https://httpstatuses.com/404",
            Detail = $"No task with id {id} was found.",
            Instance = HttpContext.Request.Path
        });

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => await _service.DeleteAsync(id, ct) ? NoContent() : NotFound(new ProblemDetails
        {
            Title = "Task not found.",
            Status = StatusCodes.Status404NotFound,
            Type = "https://httpstatuses.com/404",
            Detail = $"No task with id {id} was found.",
            Instance = HttpContext.Request.Path
        });
}

