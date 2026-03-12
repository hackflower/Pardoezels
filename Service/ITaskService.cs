public interface ITaskService
{
    Efteldingen<TaskItem> GetAllTasks();
    void AddTask(string description, string name, TaskItem.Importance priority);
    void RemoveTask(int id);
    void ChangeTaskStatus(int id, TaskItem.Progress status);
    void ChangeTaskDescription(int id, string desc);
    void ChangeTaskName(int id, string name);
    void ChangeTaskPriority(int id, TaskItem.Importance priority);
}
