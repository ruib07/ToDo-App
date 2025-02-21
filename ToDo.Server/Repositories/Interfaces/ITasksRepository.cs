using ToDo.Server.Models;
using TaskStatus = ToDo.Server.Models.TaskStatus;

namespace ToDo.Server.Repositories.Interfaces;

public interface ITasksRepository
{
    Task<List<Tasks>> GetTasksByUser(Guid userId);
    Task<Tasks> GetTaskById(Guid taskId);
    Task<Tasks> CreateTask(Tasks task);
    Task UpdateTask(Tasks task);
    Task<Tasks> UpdateTaskStatus(Guid taskId, TaskStatus taskStatus);
    Task DeleteTask(Guid taskId);
}
