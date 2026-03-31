using System.Text.Json;

public class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    public JsonTaskRepository(string filePath) => _filePath = filePath;
    public IMyCollection<TaskItem> LoadTasks()
    {
        if (!File.Exists(_filePath))
        {
            return new Efteldingen<TaskItem>();
        }
        string json = File.ReadAllText(_filePath);
        IMyCollection<TaskItem> tasks = JsonSerializer.Deserialize<IMyCollection<TaskItem>>(json) ?? new Efteldingen<TaskItem>();

        return tasks;
    }

    public void SaveTasks(IMyCollection<TaskItem> tasks)
    {
        string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}