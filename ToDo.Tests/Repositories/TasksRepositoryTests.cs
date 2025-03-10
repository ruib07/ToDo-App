using Microsoft.EntityFrameworkCore;
using ToDo.Server.Data;
using ToDo.Server.Repositories;
using ToDo.Tests.Templates;
using TaskStatus = ToDo.Server.Models.TaskStatus;

namespace ToDo.Tests.Repositories;

[TestFixture]
public class TasksRepositoryTests
{
    private TasksRepository _tasksRepository;
    private ToDoDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ToDoDbContext>()
                     .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        _context = new ToDoDbContext(options);
        _tasksRepository = new TasksRepository(_context);
    }

    [Test]
    public async Task GetTasksByUser_ReturnsTasks()
    {
        var tasks = TasksTests.CreateTask();
        _context.Tasks.AddRange(tasks);
        await _context.SaveChangesAsync();

        var result = await _tasksRepository.GetTasksByUser(tasks[0].UserId);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo(tasks[0].Id));
            Assert.That(result.First().UserId, Is.EqualTo(tasks[0].UserId));
            Assert.That(result.First().Title, Is.EqualTo(tasks[0].Title));
            Assert.That(result.First().Description, Is.EqualTo(tasks[0].Description));
            Assert.That(result.First().Status, Is.EqualTo(tasks[0].Status));
            Assert.That(result.First().DueDate, Is.EqualTo(tasks[0].DueDate));
        });
    }

    [Test]
    public async Task GetTaskById_ReturnsTask()
    {
        var task = TasksTests.CreateTask().First();

        await _tasksRepository.CreateTask(task);

        var result = await _tasksRepository.GetTaskById(task.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(task.Id));
            Assert.That(result.UserId, Is.EqualTo(task.UserId));
            Assert.That(result.Title, Is.EqualTo(task.Title));
            Assert.That(result.Description, Is.EqualTo(task.Description));
            Assert.That(result.Status, Is.EqualTo(task.Status));
            Assert.That(result.DueDate, Is.EqualTo(task.DueDate));
        });
    }

    [Test]
    public async Task CreateTask_CreatesSuccessfully()
    {
        var newTask = TasksTests.CreateTask().First();

        var result = await _tasksRepository.CreateTask(newTask);
        var addedTask = await _tasksRepository.GetTaskById(newTask.Id);

        Assert.That(addedTask, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(newTask.Id));
            Assert.That(result.UserId, Is.EqualTo(newTask.UserId));
            Assert.That(result.Title, Is.EqualTo(newTask.Title));
            Assert.That(result.Description, Is.EqualTo(newTask.Description));
            Assert.That(result.Status, Is.EqualTo(newTask.Status));
            Assert.That(result.DueDate, Is.EqualTo(newTask.DueDate));
        });
    }

    [Test]
    public async Task UpdateTask_UpdatesSuccessfully()
    {
        var existingTask = TasksTests.CreateTask().First();
        await _tasksRepository.CreateTask(existingTask);

        _context.Entry(existingTask).State = EntityState.Detached;

        var updatedTask = TasksTests.UpdateTask(existingTask.Id, existingTask.UserId);

        await _tasksRepository.UpdateTask(updatedTask);
        var retrievedUpdatedTask = await _tasksRepository.GetTaskById(existingTask.Id);

        Assert.That(retrievedUpdatedTask, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(retrievedUpdatedTask.Id, Is.EqualTo(updatedTask.Id));
            Assert.That(retrievedUpdatedTask.UserId, Is.EqualTo(updatedTask.UserId));
            Assert.That(retrievedUpdatedTask.Title, Is.EqualTo(updatedTask.Title));
            Assert.That(retrievedUpdatedTask.Description, Is.EqualTo(updatedTask.Description));
            Assert.That(retrievedUpdatedTask.Status, Is.EqualTo(updatedTask.Status));
            Assert.That(retrievedUpdatedTask.DueDate, Is.EqualTo(updatedTask.DueDate));
        });
    }

    [Test]
    public async Task UpdateTaskStatus_UpdatesSuccessfully()
    {
        var existingTask = TasksTests.CreateTask().First();
        var updateTaskStatus = TaskStatus.Completed;
        await _tasksRepository.CreateTask(existingTask);

        var updatedTask = await _tasksRepository.UpdateTaskStatus(existingTask.Id, updateTaskStatus);

        Assert.That(updatedTask, Is.Not.Null);
        Assert.That(updatedTask.Status, Is.EqualTo(TaskStatus.Completed));
    }

    [Test]
    public async Task DeleteTask_DeletesSuccessfully()
    {
        var existingTask = TasksTests.CreateTask().First();

        await _tasksRepository.CreateTask(existingTask);
        await _tasksRepository.DeleteTask(existingTask.Id);
        var retrivedEmptyTask = await _tasksRepository.GetTaskById(existingTask.Id);

        Assert.That(retrivedEmptyTask, Is.Null);
    }
}
