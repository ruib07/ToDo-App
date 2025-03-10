using Moq;
using ToDo.Server.Models;
using ToDo.Server.Repositories.Interfaces;
using ToDo.Server.Services;
using ToDo.Tests.Templates;
using TaskStatus = ToDo.Server.Models.TaskStatus;

namespace ToDo.Tests.Services;

[TestFixture]
public class TasksServiceTests
{
    private Mock<ITasksRepository> _tasksRepositoryMock;
    private TasksService _tasksService;

    [SetUp]
    public void Setup()
    {
        _tasksRepositoryMock = new Mock<ITasksRepository>();
        _tasksService = new TasksService(_tasksRepositoryMock.Object);
    }

    [Test]
    public async Task GetTasksByUser_ReturnsTasks()
    {
        var tasks = TasksTests.CreateTask();
        var singleTaskList = new List<Tasks>() { tasks[0] };

        _tasksRepositoryMock.Setup(repo => repo.GetTasksByUser(tasks[0].UserId)).ReturnsAsync(singleTaskList);

        var result = await _tasksService.GetTasksByUser(tasks[0].UserId);

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

        _tasksRepositoryMock.Setup(repo => repo.GetTaskById(task.Id)).ReturnsAsync(task);

        var result = await _tasksService.GetTaskById(task.Id);

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
    public void GetTaskById_ReturnsNotFound_WhenTaskNotFound()
    {
        _tasksRepositoryMock.Setup(repo => repo.GetTaskById(It.IsAny<Guid>())).ReturnsAsync((Tasks)null);

        var exception = Assert.ThrowsAsync<Exception>(async () => await _tasksService.GetTaskById(Guid.NewGuid()));

        Assert.That(exception.Message, Is.EqualTo("Task not found."));
    }

    [Test]
    public async Task CreateTask_CreatesSuccessfully()
    {
        var task = TasksTests.CreateTask().First();

        _tasksRepositoryMock.Setup(repo => repo.CreateTask(task)).ReturnsAsync(task);

        var result = await _tasksService.CreateTask(task);

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
    public async Task UpdateTask_UpdatesSuccessfully()
    {
        var task = TasksTests.CreateTask().First();
        var updateTask = TasksTests.UpdateTask(task.Id, task.UserId);

        _tasksRepositoryMock.Setup(repo => repo.CreateTask(task)).ReturnsAsync(task);
        _tasksRepositoryMock.Setup(repo => repo.UpdateTask(task)).Returns(Task.CompletedTask);
        _tasksRepositoryMock.Setup(repo => repo.GetTaskById(task.Id)).ReturnsAsync(task);

        await _tasksService.UpdateTask(task.Id, updateTask);
        var result = await _tasksService.GetTaskById(task.Id);

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
    public async Task UpdateTaskStatus_UpdatesSuccessfully()
    {
        var task = TasksTests.CreateTask().First();
        var updateTaskStatus = TasksTests.UpdateTaskStatus(task.Id, task.UserId, task.Title, task.Description, TaskStatus.Completed, task.DueDate);

        _tasksRepositoryMock.Setup(repo => repo.CreateTask(task)).ReturnsAsync(task);
        _tasksRepositoryMock.Setup(repo => repo.UpdateTaskStatus(task.Id, updateTaskStatus.Status)).ReturnsAsync(updateTaskStatus);
        _tasksRepositoryMock.Setup(repo => repo.GetTaskById(task.Id)).ReturnsAsync(updateTaskStatus);

        await _tasksService.UpdateTaskStatus(task.Id, updateTaskStatus.Status);
        var result = await _tasksService.GetTaskById(task.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(updateTaskStatus.Id));
            Assert.That(result.UserId, Is.EqualTo(updateTaskStatus.UserId));
            Assert.That(result.Title, Is.EqualTo(updateTaskStatus.Title));
            Assert.That(result.Description, Is.EqualTo(updateTaskStatus.Description));
            Assert.That(result.Status, Is.EqualTo(updateTaskStatus.Status));
            Assert.That(result.DueDate, Is.EqualTo(updateTaskStatus.DueDate));
        });
    }

    [Test]
    public async Task DeleteTask_DeletesSuccessfully()
    {
        var task = TasksTests.CreateTask().First();

        _tasksRepositoryMock.Setup(repo => repo.CreateTask(task)).ReturnsAsync(task);
        _tasksRepositoryMock.Setup(repo => repo.DeleteTask(task.Id)).Returns(Task.CompletedTask);
        _tasksRepositoryMock.Setup(repo => repo.GetTaskById(task.Id)).ReturnsAsync((Tasks)null);

        await _tasksService.CreateTask(task);
        await _tasksService.DeleteTask(task.Id);

        var exception = Assert.ThrowsAsync<Exception>(async () => await _tasksService.GetTaskById(task.Id));

        Assert.That(exception.Message, Is.EqualTo("Task not found."));
    }
}
