using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDo.Server.Controllers;
using ToDo.Server.Models;
using ToDo.Server.Repositories.Interfaces;
using ToDo.Server.Services;
using ToDo.Tests.Templates;
using TaskStatus = ToDo.Server.Models.TaskStatus;

namespace ToDo.Tests.Controllers;

[TestFixture]
public class TasksControllerTests
{
    private Mock<ITasksRepository> _tasksRepositoryMock;
    private TasksService _taskService;
    private TasksController _taskController;

    [SetUp]
    public void Setup()
    {
        _tasksRepositoryMock = new Mock<ITasksRepository>();
        _taskService = new TasksService(_tasksRepositoryMock.Object);
        _taskController = new TasksController(_taskService);
    }

    [Test]
    public async Task GetTasksByUser_ReturnsOkResult_WithTasks()
    { 
        var tasks = TasksTests.CreateTask();
        var singleTaskList = new List<Tasks>() { tasks[0] };

        _tasksRepositoryMock.Setup(repo => repo.GetTasksByUser(tasks[0].UserId)).ReturnsAsync(singleTaskList);

        var result = await _taskController.GetTasksByUser(tasks[0].UserId);
        var okResult = result as OkObjectResult;
        var response = okResult.Value as List<Tasks>;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response, Has.Count.EqualTo(1));
            Assert.That(response.First().Id, Is.EqualTo(tasks[0].Id));
            Assert.That(response.First().UserId, Is.EqualTo(tasks[0].UserId));
            Assert.That(response.First().Title, Is.EqualTo(tasks[0].Title));
            Assert.That(response.First().Description, Is.EqualTo(tasks[0].Description));
            Assert.That(response.First().Status, Is.EqualTo(tasks[0].Status));
            Assert.That(response.First().DueDate, Is.EqualTo(tasks[0].DueDate));
        });
    }

    [Test]
    public async Task GetTaskById_ReturnsOkResult_WithTask()
    {
        var task = TasksTests.CreateTask().First();

        _tasksRepositoryMock.Setup(repo => repo.GetTaskById(task.Id)).ReturnsAsync(task);

        var result = await _taskController.GetTaskById(task.Id);
        var okResult = result as OkObjectResult;
        var response = okResult.Value as Tasks;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response.Id, Is.EqualTo(task.Id));
            Assert.That(response.UserId, Is.EqualTo(task.UserId));
            Assert.That(response.Title, Is.EqualTo(task.Title));
            Assert.That(response.Description, Is.EqualTo(task.Description));
            Assert.That(response.Status, Is.EqualTo(task.Status));
            Assert.That(response.DueDate, Is.EqualTo(task.DueDate));
        });
    }

    [Test]
    public async Task CreateTask_ReturnsCreatedResult_WhenTaskIsCreated()
    {
        var newTask = TasksTests.CreateTask().First();

        _tasksRepositoryMock.Setup(repo => repo.CreateTask(newTask)).ReturnsAsync(newTask);

        var result = await _taskController.CreateTask(newTask);
        var createdResult = result as CreatedAtActionResult;

        Assert.That(createdResult, Is.Not.Null);
        Assert.That(createdResult.StatusCode, Is.EqualTo(201));
    }

    [Test]
    public async Task UpdateTask_ReturnsOkResult_WhenStatusIsUpdated()
    {
        var task = TasksTests.CreateTask().First();
        var updatedTask = TasksTests.UpdateTask(task.Id, task.UserId);

        _tasksRepositoryMock.Setup(repo => repo.GetTaskById(task.Id)).ReturnsAsync(task);
        _tasksRepositoryMock.Setup(repo => repo.UpdateTask(It.IsAny<Tasks>())).Returns(Task.CompletedTask);

        var result = await _taskController.UpdateTask(task.Id, updatedTask);
        var okResult = result as OkObjectResult;

        Assert.That(okResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo("Task updated successfully!"));
        });
    }

    [Test]
    public async Task UpdateTaskStatus_ReturnsOkResult_WhenTaskStatusIsUpdated()
    {
        var task = TasksTests.CreateTask().First();
        var updatedTaskStatus = TasksTests.UpdateTaskStatus(task.Id, task.UserId, task.Title, task.Description, TaskStatus.Completed, task.DueDate);

        _tasksRepositoryMock.Setup(repo => repo.GetTaskById(task.Id)).ReturnsAsync(task);
        _tasksRepositoryMock.Setup(repo => repo.UpdateTaskStatus(task.Id, updatedTaskStatus.Status)).ReturnsAsync(updatedTaskStatus);

        var result = await _taskController.UpdateTaskStatus(task.Id, updatedTaskStatus.Status);
        var okResult = result.Result as OkObjectResult;

        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult.StatusCode, Is.EqualTo(200));
    }

    [Test]
    public async Task DeleteTask_ReturnsNoContentResult_WhenTaskIsDeleted()
    {
        var task = TasksTests.CreateTask().First();

        _tasksRepositoryMock.Setup(repo => repo.GetTaskById(task.Id)).ReturnsAsync(task);
        _tasksRepositoryMock.Setup(repo => repo.DeleteTask(task.Id)).Returns(Task.CompletedTask);

        var result = await _taskController.DeleteTask(task.Id);
        var noContentResult = result as NoContentResult;

        Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
    }
}
