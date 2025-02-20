using ToDo.Server.Models;

namespace ToDo.Server.Repositories.Interfaces;

public interface ITasksRepository
{
    Task<List<Tasks>> GetTasksByUser(Guid userId);
    Task<Tasks> GetTaskById(Guid taskId);
    Task<Tasks> CreateTask(Tasks task);
    Task UpdateTask(Tasks task);
    Task DeleteTask(Guid taskId);
}
