using Microsoft.AspNetCore.Mvc;
using TaskMasterAPI.Models;

namespace TaskMasterAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
	// Temporary in-memory storage(to-be replaced by EF core)
	private static readonly List<TaskItem> Tasks =
	[
		new TaskItem { Id = 1, Title = "Fist task", IsDone = false },
		new TaskItem { Id = 2, Title = "Ship TasksController", IsDone = true }
	];

	[HttpGet]
	public ActionResult<IEnumerable<TaskItem>> GetAll() => Ok(Tasks);

	[HttpGet("{id:int}")]
	public ActionResult<TaskItem> GetById(int id)
	{
		var task = Tasks.FirstOrDefault(t => t.Id == id);
		return task is null ? NotFound() : Ok(task);
	}

	[HttpPost]
	public ActionResult<TaskItem> Create([FromBody] TaskItem input)
	{
		var nextId = Tasks.Count == 0 ? 1 : Tasks.Max(t => t.Id) + 1;
		var created = new TaskItem { Id = nextId, Title = input.Title, IsDone = input.IsDone };
		Tasks.Add(created);
		return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
	}
}
