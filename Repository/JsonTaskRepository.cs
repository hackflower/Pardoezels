using System.Text.Json;

public class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    private readonly IMyCollection<TaskItem> _tasks;

    public JsonTaskRepository(string filePath, IMyCollection<TaskItem> tasks)
    {
        _filePath = filePath;
        _tasks = tasks;
    }

    public IMyCollection<TaskItem> LoadTasks()
    {
        if (!File.Exists(_filePath))
        {
            return _tasks;
        }
        string json = File.ReadAllText(_filePath);
        TaskItem[] array = JsonSerializer.Deserialize<TaskItem[]>(json) ?? Array.Empty<TaskItem>();
        IMyCollection<TaskItem> tasks = _tasks.FromArray(array);

        return tasks;
    }

    public void SaveTasks(IMyCollection<TaskItem> tasks)
    {
        string json = JsonSerializer.Serialize(tasks.ToArray(), new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}