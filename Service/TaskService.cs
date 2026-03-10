public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly Efteldingen<TaskItem> _tasks;
    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
        _tasks = _repository.LoadTasks();
    }
    public IEnumerable<TaskItem> GetAllTasks() => _tasks;
    public void AddTask(string description, string name, string priority)
    {
        int newId = _tasks.Count > 0 ? _tasks[_tasks.Count - 1].Id + 1 :
       1;
        var newTask = new TaskItem
        {
            Id = newId,
            Name = name,
            Description =
            description,
            CreatedAt = DateTime.Now,
            Completed = false
        };
        _tasks.Add(newTask);
        _repository.SaveTasks(_tasks);
    }
    public void RemoveTask(int id)
    {
        var task = _tasks.Find(id, (t, i) => t.Id == id);
        if (task.HasValue)
        {
            _tasks.Remove(task.Value);
            _repository.SaveTasks(_tasks);
        }
    }
    public void ToggleTaskCompletion(int id)
    {
        var task = _tasks.Find(id, (t, i) => t.Id == id);
        if (task.HasValue)
        {
            task.Value.Completed = !task.Value.Completed;
            _repository.SaveTasks(_tasks);
        }
    }
    public void ChangeTaskName(int id, string name)
    {
        var task = _tasks.Find(id, (t, i) => t.Id == id);
        if (task.HasValue)
        {
            task.Value.Name = name;
            _repository.SaveTasks(_tasks);
        }
    }

    public void ChangeTaskDescription(int id, string desc)
    {
        var task = _tasks.Find(id, (t, i) => t.Id == id);
        if (task.HasValue)
        {
            task.Value.Description = desc;
            _repository.SaveTasks(_tasks);
        }
    }
}
