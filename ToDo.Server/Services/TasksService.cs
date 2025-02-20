using ToDo.Server.Models;
using ToDo.Server.Repositories.Interfaces;

namespace ToDo.Server.Services;

public class TasksService
{
    private readonly ITasksRepository _tasksRepository;

    public TasksService(ITasksRepository tasksRepository)
    {
        _tasksRepository = tasksRepository;
    }

    public async Task<List<Tasks>> GetTasksByUser(Guid userId)
    {
        var tasksByUser = await _tasksRepository.GetTasksByUser(userId);

        if (tasksByUser == null || tasksByUser.Count == 0) throw new Exception("No Tasks found for this user.");

        return tasksByUser;
    }

    public async Task<Tasks> GetTaskById(Guid taskId)
    {
        var task = await _tasksRepository.GetTaskById(taskId) ?? throw new Exception("Task not found");

        return task;
    }

    public async Task<Tasks> CreateTask(Tasks task)
    {
        return await _tasksRepository.CreateTask(task);
    }

    public async Task<Tasks> UpdateTask(Guid taskId, Tasks updateTask)
    {
        var currentTask = await GetTaskById(taskId);

        currentTask.Title = updateTask.Title;
        currentTask.Description = updateTask.Description;
        currentTask.Status = updateTask.Status;
        currentTask.DueDate = updateTask.DueDate;

        await _tasksRepository.UpdateTask(currentTask);
        return currentTask;
    }

    public async Task DeleteTask(Guid taskId)
    {
        await _tasksRepository.DeleteTask(taskId);
    }
}
