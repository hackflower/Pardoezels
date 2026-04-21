class Program
{
    static void Main(string[] args)
    {
        string filePath = "tasks.json";

        IMyCollection<TaskItem> tasks = new Efteldingen<TaskItem>();
        IMyCollection<User> users = new Efteldingen<User>();

        ITaskRepository repository = new JsonTaskRepository(filePath, tasks);
        IUserRepository userRepository = new JsonUserRepository("users.json", users);
        ITaskService taskService = new TaskService(repository);
        IUserService userService = new UserService(userRepository);
        ITaskView view = new ConsoleTaskView(taskService, userService);
        
        view.Run();
    }
}