namespace ToDo.Server.Models;

public enum TaskStatus
{
    Pending = 0,
    Completed = 1
};

public class Tasks
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Users User { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
