interface ITaskRepository
{
    Efteldingen<TaskItem> LoadTasks();
    void SaveTasks(Efteldingen<TaskItem> tasks);
}
