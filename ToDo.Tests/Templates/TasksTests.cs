using ToDo.Server.Models;
using TaskStatus = ToDo.Server.Models.TaskStatus;

namespace ToDo.Tests.Templates;

public static class TasksTests
{
    public static List<Tasks> CreateTask()
    {
        return new List<Tasks>()
        {
            new Tasks()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Title = "Task1 Test",
                Description = "Task1 Description Test",
                Status = TaskStatus.Pending,
                DueDate = DateTime.UtcNow
            },
            new Tasks()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Title = "Task2 Test",
                Description = "Task2 Description Test",
                Status = TaskStatus.Pending,
                DueDate = DateTime.UtcNow.AddDays(2)
            }
        };
    }

    public static Tasks UpdateTaskStatus(Guid id, Guid userId, string title, string description, TaskStatus status, DateTime dueDate)
    {
        return new Tasks()
        {
            Id = id,
            UserId = userId,
            Title = title,
            Description = description,
            Status = status,
            DueDate = dueDate
        };
    }

    public static Tasks UpdateTask(Guid id, Guid userId)
    {
        return new Tasks()
        {
            Id = id,
            UserId = userId,
            Title = "Task Updated",
            Description = "Task Description Updated",
            Status = TaskStatus.Completed,
            DueDate = DateTime.UtcNow.AddDays(1)
        };
    }
}
