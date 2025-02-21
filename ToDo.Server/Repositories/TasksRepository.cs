using Microsoft.EntityFrameworkCore;
using ToDo.Server.Data;
using ToDo.Server.Models;
using TaskStatus = ToDo.Server.Models.TaskStatus;
using ToDo.Server.Repositories.Interfaces;

namespace ToDo.Server.Repositories;

public class TasksRepository : ITasksRepository
{
    private readonly ToDoDbContext _context;
    private DbSet<Tasks> Tasks => _context.Tasks;

    public TasksRepository(ToDoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Tasks>> GetTasksByUser(Guid userId)
    {
        return await Tasks.Where(t => t.UserId == userId).ToListAsync();
    }

    public async Task<Tasks> GetTaskById(Guid taskId)
    {
        return await Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
    }

    public async Task<Tasks> CreateTask(Tasks task)
    {
        await Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        return task;
    }

    public async Task UpdateTask(Tasks task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task<Tasks> UpdateTaskStatus(Guid taskId, TaskStatus taskStatus)
    {
        var existingTask = await Tasks.FirstOrDefaultAsync(t => t.Id == taskId);

        if (existingTask == null) return null;

        existingTask.Status = taskStatus;
        await _context.SaveChangesAsync();

        return existingTask;
    }

    public async Task DeleteTask(Guid taskId)
    {
        var deleteTask = await GetTaskById(taskId);

        Tasks.Remove(deleteTask);
        await _context.SaveChangesAsync();
    }
}
