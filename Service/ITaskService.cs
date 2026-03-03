public interface ITaskService
{
    IEnumerable<TaskItem> GetAllTasks();
    void AddTask(string description, string name);
    void RemoveTask(int id);
    void ToggleTaskCompletion(int id);
    void ChangeTaskDescription(int id, string desc);
    void ChangeTaskName(int id, string name);
}
