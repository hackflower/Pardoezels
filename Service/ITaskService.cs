public interface ITaskService
{
    Efteldingen<TaskItem> GetAllTasks();
    void AddTask(string description, string name, string priority, TaskItem.Progress status = TaskItem.Progress.NotStarted);
    void RemoveTask(int id);
    void ChangeTaskStatus(int id, TaskItem.Progress status);
    void ChangeTaskDescription(int id, string desc);
    void ChangeTaskName(int id, string name);
    void ChangeTaskPriority(int id, string priority);
}
