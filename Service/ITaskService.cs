public interface ITaskService
{
    Efteldingen<TaskItem> GetAllTasks();
    void AddTask(string name, string descriptions, TaskItem.Importance priority, Efteldingen<User> users);
    void RemoveTask(int id);
    void ChangeTaskStatus(int id, TaskItem.Progress status);
    void ChangeTaskDescription(int id, string desc);
    void ChangeTaskName(int id, string name);
    void ChangeTaskPriority(int id, TaskItem.Importance priority);
    void AddAssignedUser(int id, User user);
}
