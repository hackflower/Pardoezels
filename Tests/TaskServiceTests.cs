using Xunit;
using System;
using System.IO;
using System.Linq;

namespace Tests;

public class TaskServiceTests
{
    private string GetTempFilePath()
    {
        return Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
    }

    [Fact]
    public void RemoveTaskTest()
    {
        string filePath = GetTempFilePath();

        ITaskRepository repository = new JsonTaskRepository(filePath);
        ITaskService service = new TaskService(repository);

        service.AddTask("test description", "test task", TaskItem.Importance.High);

        var allTasks = service.GetAllTasks().ToList();
        Assert.Single(allTasks);
        var taskId = allTasks[0].Id;

        service.RemoveTask(taskId);

        allTasks = service.GetAllTasks().ToList();
        Assert.Empty(allTasks);

        if (File.Exists(filePath))
            File.Delete(filePath);
    }

    [Fact]
    public void ToggleTaskCompletionTest()
    {
        string filePath = GetTempFilePath();

        ITaskRepository repository = new JsonTaskRepository(filePath);
        ITaskService service = new TaskService(repository);

        service.AddTask("test description", "test task", TaskItem.Importance.High);

        var allTasks = service.GetAllTasks().ToList();
        Assert.Single(allTasks);
        var taskId = allTasks[0].Id;

        service.ChangeTaskStatus(taskId, TaskItem.Progress.Completed);

        allTasks = service.GetAllTasks().ToList();
        Assert.Single(allTasks);
        Assert.True(allTasks[0].Status == TaskItem.Progress.Completed);

        if (File.Exists(filePath))
            File.Delete(filePath);
    }

    [Fact]
    public void ChangeTaskDescriptionTest()
    {
        string filePath = GetTempFilePath();

        ITaskRepository repository = new JsonTaskRepository(filePath);
        ITaskService service = new TaskService(repository);

        service.AddTask("test description", "test task", TaskItem.Importance.High);

        var allTasks = service.GetAllTasks().ToList();
        Assert.Single(allTasks);
        var taskId = allTasks[0].Id;

        service.ChangeTaskDescription(taskId, "new description");

        allTasks = service.GetAllTasks().ToList();
        Assert.Single(allTasks);
        Assert.Equal("new description", allTasks[0].Description);

        if (File.Exists(filePath))
            File.Delete(filePath);
    }

    [Fact]
    public void ChangeTaskNameTest()
    {
        string filePath = GetTempFilePath();

        ITaskRepository repository = new JsonTaskRepository(filePath);
        ITaskService service = new TaskService(repository);

        service.AddTask("test description", "test task", TaskItem.Importance.High);

        var allTasks = service.GetAllTasks().ToList();
        Assert.Single(allTasks);
        var taskId = allTasks[0].Id;

        service.ChangeTaskName(taskId, "new name");

        allTasks = service.GetAllTasks().ToList();
        Assert.Single(allTasks);
        Assert.Equal("new name", allTasks[0].Name);

        if (File.Exists(filePath))
            File.Delete(filePath);
    }

    [Fact]
    public void AddTaskTest()
    {
        string filePath = GetTempFilePath();

        ITaskRepository repository = new JsonTaskRepository(filePath);
        ITaskService service = new TaskService(repository);

        service.AddTask("test description", "test task", TaskItem.Importance.High);

        var allTasks = service.GetAllTasks().ToList();
        Assert.Single(allTasks);
        Assert.Equal("test description", allTasks[0].Description);
        Assert.Equal("test task", allTasks[0].Name);
        Assert.Equal(TaskItem.Progress.NotStarted, allTasks[0].Status);

        if (File.Exists(filePath))
            File.Delete(filePath);
    }

    [Fact]
    public void GetAllTasksTest()
    {
        string filePath = GetTempFilePath();

        ITaskRepository repository = new JsonTaskRepository(filePath);
        ITaskService service = new TaskService(repository);

        service.AddTask("desc 1", "task 1", TaskItem.Importance.High);
        service.AddTask("desc 2", "task 2", TaskItem.Importance.Normal);

        var allTasks = service.GetAllTasks().ToList();
        Assert.Equal(2, allTasks.Count);
        Assert.Equal("desc 1", allTasks[0].Description);
        Assert.Equal("task 1", allTasks[0].Name);
        Assert.Equal("desc 2", allTasks[1].Description);
        Assert.Equal("task 2", allTasks[1].Name);

        if (File.Exists(filePath))
            File.Delete(filePath);
    }
}