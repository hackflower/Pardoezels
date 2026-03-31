using System.Text.Json;

public class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    public JsonTaskRepository(string filePath) => _filePath = filePath;
    public Efteldingen<TaskItem> LoadTasks()
    {
        if (!File.Exists(_filePath))
        {
            return new Efteldingen<TaskItem>();
        }
        string json = File.ReadAllText(_filePath);
        TaskItem[] array = JsonSerializer.Deserialize<TaskItem[]>(json) ?? Array.Empty<TaskItem>();
        Efteldingen<TaskItem> tasks = (Efteldingen<TaskItem>)new Efteldingen<TaskItem>().FromArray(array);

        return tasks;
    }

    public void SaveTasks(Efteldingen<TaskItem> tasks)
    {
        string json = JsonSerializer.Serialize(tasks.ToArray(), new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}