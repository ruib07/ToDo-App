using Microsoft.AspNetCore.Mvc;
using ToDo.Server.Models;
using ToDo.Server.Services;

namespace ToDo.Server.Controllers;

[Route("api/v1/tasks")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly TasksService _tasksService;

    public TasksController(TasksService tasksService)
    {
        _tasksService = tasksService;
    }

    // GET api/v1/tasks/byuser/{userId}
    [HttpGet("byuser/{userId}")]
    public async Task<IActionResult> GetTasksByUser([FromRoute] Guid userId)
    {
        var tasks = await _tasksService.GetTasksByUser(userId);

        return Ok(tasks);
    }

    // GET api/v1/tasks/{taskId}
    [HttpGet("{taskId}")]
    public async Task<IActionResult> GetTaskById([FromRoute] Guid taskId)
    {
        var task = await _tasksService.GetTaskById(taskId);

        return Ok(task);
    }

    // POST api/v1/tasks
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] Tasks task)
    {
        var createdTask = await _tasksService.CreateTask(task);

        return CreatedAtAction(nameof(GetTaskById), new { taskId = createdTask.Id }, new
        {
            Message = "Task created successfully!",
            createdTask.Id
        });
    }

    // PUT api/v1/tasks/{taskId}
    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTask([FromRoute] Guid taskId, [FromBody] Tasks updateTask)
    {
        await _tasksService.UpdateTask(taskId, updateTask);

        return Ok("Task updated successfully!");
    }

    // DELETE api/v1/tasks/{taskId}
    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTask([FromRoute] Guid taskId)
    {
        await _tasksService.DeleteTask(taskId);

        return NoContent();
    }
}
