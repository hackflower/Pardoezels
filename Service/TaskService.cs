public class TaskService : ITaskService
{
    private readonly UserService _userService;
    private readonly ITaskRepository _repository;
    public IMyCollection<TaskItem> tasks;
    public TaskService(ITaskRepository repository, UserService userService)
    {
        _userService = userService;
        _repository = repository;
        tasks = _repository.LoadTasks();
    }
    public IMyCollection<TaskItem> GetAllTasks() => tasks;
    public void AddTask(string name, string description, TaskItem.Importance priority, int userId)
    {
        var newTask = new TaskItem(new Efteldingen<int>(), new Efteldingen<int>())
        {
            Id = tasks.Count > 0 ? tasks.Max((a, b) => a.Id.CompareTo(b.Id)).Id + 1 : 1,
            Name = name,
            Description = description,
            CreatedAt = DateTime.Now,
            Status = TaskItem.Progress.NotStarted,
            Priority = priority,
        };

        tasks.Add(newTask);
        AddAssignedUser(newTask.Id, _userService.GetUserById(userId)!);
        _repository.SaveTasks(tasks);
    }

    public void RemoveTask(int id)
    {
        var task = tasks.FindBy(id, (t, i) => t.Id.CompareTo(i));
        if (task.HasValue)
        {
            tasks.Remove(task.Value);
            _repository.SaveTasks(tasks);
        }
    }

    public void ChangeTaskStatus(int id, TaskItem.Progress status)
    {
        var task = tasks.FindBy(id, (t, i) => t.Id.CompareTo(i));
        if (task.HasValue)
        {
            task.Value.Status = status;
            _repository.SaveTasks(tasks);
        }
    }

    public void ChangeTaskName(int id, string name)
    {
        var task = tasks.FindBy(id, (t, i) => t.Id.CompareTo(i));
        if (task.HasValue)
        {
            task.Value.Name = name;
            _repository.SaveTasks(tasks);
        }
    }

    public void ChangeTaskDescription(int id, string desc)
    {
        var task = tasks.FindBy(id, (t, i) => t.Id.CompareTo(i));
        if (task.HasValue)
        {
            task.Value.Description = desc;
            _repository.SaveTasks(tasks);
        }
    }

    public void ChangeTaskPriority(int id, TaskItem.Importance priority)
    {
        var task = tasks.FindBy(id, (t, i) => t.Id.CompareTo(i));
        if (task.HasValue)
        {
            task.Value.Priority = priority;
            _repository.SaveTasks(tasks);
        }
    }

    public void AddAssignedUser(int id, User user)
    {
        var task = tasks.FindBy(id, (t, i) => t.Id.CompareTo(i));
        if (task.HasValue)
        {
            task.Value.AssignedUsersIds.Add(user.Id);
            _repository.SaveTasks(tasks);
        }
    }

    public void AddDependency(int id, TaskItem task)
    {
        var taskToAdd = tasks.FindBy(id, (t, i) => t.Id.CompareTo(i));
        if (taskToAdd.HasValue)
        {
            task.DependenciesIds.Add(taskToAdd.Value.Id);
            _repository.SaveTasks(tasks);
        }
    }

    public void RemoveDependency(int id, TaskItem task)
    {
        var taskToRemove = tasks.FindBy(id, (t, i) => t.Id.CompareTo(i));
        if (taskToRemove.HasValue)
        {
            task.DependenciesIds.Remove(taskToRemove.Value.Id);
            _repository.SaveTasks(tasks);
        }
    }

    public TaskItem GetTaskById(int id)
    {
        var task = tasks.FindBy(id, (t, i) => t.Id.CompareTo(i));
        if (task.HasValue) return task.Value;

        throw new ArgumentException("Task not found");
    }
}
