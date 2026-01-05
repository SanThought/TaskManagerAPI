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
    public ActionResult<IEnumerable<TaskReadDto>> GetAll()
    {
        var tasks = _service.GetAll();
        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public ActionResult<TaskReadDto> GetById(int id)
    {
        var task = _service.GetById(id);
        return task is null ? NotFound() : Ok(task);
    }

    [HttpPost]
    public ActionResult<TaskReadDto> Create([FromBody] TaskCreateDto dto)
    {
        // With [ApiController] + FluentValidation auto-validation, invalid DTO => automatic 400
        var created = _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // Optional today (small and useful)
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] TaskUpdateDto dto)
    {
        var ok = _service.Update(id, dto);
        return ok ? NoContent() : NotFound();
    }

    // Optional today (small and useful)
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var ok = _service.Delete(id);
        return ok ? NoContent() : NotFound();
    }
}

