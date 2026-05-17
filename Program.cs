class Program
{
    static void Main()
    {
        string filePath = "tasks.json";

        int choice = ConsoleTaskView.SelectOption("==== Collection implementation ====", ["Efteldingen", "Eftelinked", "BinaryFairytaleTree"], "");

        IMyCollection<TaskItem> tasks;
        IMyCollection<User> users;

        if (choice == 0)
        {
            tasks = new Efteldingen<TaskItem>();
            users = new Efteldingen<User>();
        }
        else if (choice == 1)
        {
            tasks = new Eftelinked<TaskItem>();
            users = new Eftelinked<User>();
        }
        else
        {
            tasks = new BinaryFairytaleTree<TaskItem>(Comparer<TaskItem>
                .Create((a, b) => a.Name.CompareTo(b.Name)));
            users = new BinaryFairytaleTree<User>(Comparer<User>
                .Create((a, b) => a.Username.CompareTo(b.Username)));
        }

        ITaskRepository taskRepository = new JsonTaskRepository(filePath, tasks);
        IUserRepository userRepository = new JsonUserRepository("users.json", users);
        TaskService taskService = new TaskService(taskRepository);
        UserService userService = new UserService(userRepository);
        ITaskView view = new ConsoleTaskView(taskService, userService);
        
        view.Run();
    }
}