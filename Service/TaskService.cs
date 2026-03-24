public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly Efteldingen<TaskItem> _tasks;
    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
        _tasks = _repository.LoadTasks();
    }
    public Efteldingen<TaskItem> GetAllTasks() => _tasks;
    public void AddTask(string name, string description, TaskItem.Importance priority)
    {
        var newTask = new TaskItem
        {
            Id = _tasks.Max((a, b) => a.Id.CompareTo(b.Id)).Id + 1,
            Name = name,
            Description = description,
            CreatedAt = DateTime.Now,
            Status = TaskItem.Progress.NotStarted,
            Priority = priority
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
    public void ChangeTaskStatus(int id, TaskItem.Progress status)
    {
        var task = _tasks.Find(id, (t, i) => t.Id == id);
        if (task.HasValue)
        {
            task.Value.Status = status;
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

    public void ChangeTaskPriority(int id, TaskItem.Importance priority)
    {
        var task = _tasks.Find(id, (t, i) => t.Id == id);
        if (task.HasValue)
        {
            task.Value.Priority = priority;
            _repository.SaveTasks(_tasks);
        }
    }
}
