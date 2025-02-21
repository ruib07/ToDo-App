using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Server.Models;
using ToDo.Server.Services;
using TaskStatus = ToDo.Server.Models.TaskStatus;

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
    [Authorize(Policy = Constants.Constants.PolicyUser)]
    [HttpGet("byuser/{userId}")]
    public async Task<IActionResult> GetTasksByUser([FromRoute] Guid userId)
    {
        var tasks = await _tasksService.GetTasksByUser(userId);

        return Ok(tasks);
    }

    // GET api/v1/tasks/{taskId}
    [Authorize(Policy = Constants.Constants.PolicyUser)]
    [HttpGet("{taskId}")]
    public async Task<IActionResult> GetTaskById([FromRoute] Guid taskId)
    {
        var task = await _tasksService.GetTaskById(taskId);

        return Ok(task);
    }

    // POST api/v1/tasks
    [Authorize(Policy = Constants.Constants.PolicyUser)]
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
    [Authorize(Policy = Constants.Constants.PolicyUser)]
    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTask([FromRoute] Guid taskId, [FromBody] Tasks updateTask)
    {
        await _tasksService.UpdateTask(taskId, updateTask);

        return Ok("Task updated successfully!");
    }

    // PATCH api/v1/tasks/{taskId}/status
    [Authorize(Policy = Constants.Constants.PolicyUser)]
    [HttpPatch("{taskId}/status")]
    public async Task<ActionResult<Tasks>> UpdateTaskStatus(Guid taskId, [FromBody] TaskStatus status)
    {
        var updatedTask = await _tasksService.UpdateTaskStatus(taskId, status);

        return Ok(new
        {
            Message = "Task status updated successfully.",
            UpdatedStatus = updatedTask.Status
        });
    }

    // DELETE api/v1/tasks/{taskId}
    [Authorize(Policy = Constants.Constants.PolicyUser)]
    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTask([FromRoute] Guid taskId)
    {
        await _tasksService.DeleteTask(taskId);

        return NoContent();
    }
}
