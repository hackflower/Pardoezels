public interface ITaskRepository
{
    IMyCollection<TaskItem> LoadTasks();
    void SaveTasks(IMyCollection<TaskItem> tasks);
}
