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
    public void AddTask(string name, string description, TaskItem.Importance priority, int userId)
    {
        var newTask = new TaskItem
        {
            Id = _tasks.Count > 0 ? _tasks.Max((a, b) => a.Id.CompareTo(b.Id)).Id + 1 : 1,
            Name = name,
            Description = description,
            CreatedAt = DateTime.Now,
            Status = TaskItem.Progress.NotStarted,
            Priority = priority,
            AssignedUsersIds = [userId],
        };

        _tasks.Add(newTask);
        _repository.SaveTasks(_tasks);
    }

    public void RemoveTask(int id)
    {
        var task = _tasks.FindBy(id, (t, i) => t.Id.CompareTo(i));
        if (task.HasValue)
        {
            _tasks.Remove(task.Value);
            _repository.SaveTasks(_tasks);
        }
    }

    public void ChangeTaskStatus(int id, TaskItem.Progress status)
    {
        var task = _tasks.FindBy(id, (t, i) => t.Id.CompareTo(i));
        if (task.HasValue)
        {
            task.Value.Status = status;
            _repository.SaveTasks(_tasks);
        }
    }

    public void ChangeTaskName(int id, string name)
    {
        var task = _tasks.FindBy(id, (t, i) => t.Id.CompareTo(i));
        if (task.HasValue)
        {
            task.Value.Name = name;
            _repository.SaveTasks(_tasks);
        }
    }

    public void ChangeTaskDescription(int id, string desc)
    {
        var task = _tasks.FindBy(id, (t, i) => t.Id.CompareTo(i));
        if (task.HasValue)
        {
            task.Value.Description = desc;
            _repository.SaveTasks(_tasks);
        }
    }

    public void ChangeTaskPriority(int id, TaskItem.Importance priority)
    {
        var task = _tasks.FindBy(id, (t, i) => t.Id.CompareTo(i));
        if (task.HasValue)
        {
            task.Value.Priority = priority;
            _repository.SaveTasks(_tasks);
        }
    }

    public void AddAssignedUser(int id, User user)
    {
        var task = _tasks.FindBy(id, (t, i) => t.Id.CompareTo(i));
        if (task.HasValue)
        {
            task.Value.AssignedUsersIds.Add(user.Id);
            _repository.SaveTasks(_tasks);
        }
    }

    public void AddDependency(int id, TaskItem task)
    {
        var taskToAdd = _tasks.FindBy(id, (t, i) => t.Id.CompareTo(i));
        if (taskToAdd.HasValue)
        {
            task.DependenciesIds.Add(taskToAdd.Value.Id);
            _repository.SaveTasks(_tasks);
        }
    }

    public void RemoveDependency(int id, TaskItem task)
    {
        var taskToRemove = _tasks.FindBy(id, (t, i) => t.Id.CompareTo(i));
        if (taskToRemove.HasValue)
        {
            task.DependenciesIds.Remove(taskToRemove.Value.Id);
            _repository.SaveTasks(_tasks);
        }
    }

    public TaskItem GetTaskById(int id)
    {
        var task = _tasks.FindBy(id, (t, i) => t.Id.CompareTo(i));
        if (task.HasValue) return task.Value;

        throw new ArgumentException("Task not found");
    }
}
