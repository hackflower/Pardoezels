public interface ITaskService
{
    IMyCollection<TaskItem> GetAllTasks();
    void AddTask(string name, string descriptions, TaskItem.Importance priority, int user);
    void RemoveTask(int id);
    void ChangeTaskStatus(int id, TaskItem.Progress status);
    void ChangeTaskDescription(int id, string desc);
    void ChangeTaskName(int id, string name);
    void ChangeTaskPriority(int id, TaskItem.Importance priority);
    void AddAssignedUser(int id, User user);
    void AddDependency(int id, TaskItem task);
    TaskItem GetTaskById(int id);
}
