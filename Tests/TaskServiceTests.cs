
using Xunit;
namespace Tests;

public class TaskServiceTests
{
    [Fact]
    public void RemoveTaskTest()
    {
        string filePath = "tasks.json";

        ITaskRepository repository = new JsonTaskRepository(filePath);
        ITaskService service = new TaskService(repository);

        Efteldingen<TaskItem> tasks = new Efteldingen<TaskItem>();

        var testTask = new TaskItem
        {
            Id = 1,
            Name = "test",
            Description =
            "test",
            CreatedAt = DateTime.Now,
            Completed = false
        };
        tasks[0] = testTask;

        service.RemoveTask(1);

        Assert.Equal(0, tasks.Count);
    }
}
