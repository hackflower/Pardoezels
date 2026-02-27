using System.Text.Json;

class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    public JsonTaskRepository(string filePath) => _filePath =
   filePath;
    public Efteldingen<TaskItem> LoadTasks()
    {
        if (!File.Exists(_filePath))
        {
            return new Efteldingen<TaskItem>();
        }
        string json = File.ReadAllText(_filePath);
        var list = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        
        Efteldingen<TaskItem> tasks = new Efteldingen<TaskItem>();

        foreach (TaskItem task in list)
        {
            tasks.Add(task);
        }

        return tasks;
    }

    public void SaveTasks(Efteldingen<TaskItem> tasks)
    {
        string json = JsonSerializer.Serialize(tasks, new
       JsonSerializerOptions
        { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}