class Program
{
    static void Main()
    {
        string filePath = "tasks.json";

        int choice = ConsoleTaskView.SelectOption("==== Collection implementation ====", ["Efteldingen", "Eftelinked"]);

        IMyCollection<TaskItem> tasks;
        IMyCollection<User> users;

        if (choice == 0)
        {
            tasks = new Efteldingen<TaskItem>();
            users = new Efteldingen<User>();
        }
        else
        {
            tasks = new Eftelinked<TaskItem>();
            users = new Eftelinked<User>();
        }

        ITaskRepository taskRepository = new JsonTaskRepository(filePath, tasks);
        IUserRepository userRepository = new JsonUserRepository("users.json", users);
        TaskService taskService = new TaskService(taskRepository);
        UserService userService = new UserService(userRepository);
        ITaskView view = new ConsoleTaskView(taskService, userService);
        
        view.Run();
    }
}