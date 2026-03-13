class Program
{
    static void Main(string[] args)
    {
        string filePath = "tasks.json";
        ITaskRepository repository = new JsonTaskRepository(filePath);
        IUserRepository userRepository = new JsonUserRepository("users.json");
        ITaskService taskService = new TaskService(repository);
        IUserService userService = new UserService(userRepository);
        ITaskView view = new ConsoleTaskView(taskService, userService);
        
        view.Run();
    }
}