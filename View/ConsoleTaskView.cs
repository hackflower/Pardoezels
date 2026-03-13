public class ConsoleTaskView : ITaskView
{
    public string? loggedInUser { get; set; }
    private readonly ITaskService _service;
    private readonly IUserService _userService;

    public enum TaskFilter
    {
        Id,
        Name,
        Description,
        Priority,
        Status
    }

    public ConsoleTaskView(ITaskService taskService, IUserService userService)
    {
        _service = taskService;
        _userService = userService;
    }

    public void DisplayTasks(int amount, int offset = 0)
    {
        Efteldingen<TaskItem> allTasks = _service.GetAllTasks();

        for (int i = offset; i < amount + offset; i++)
        {
            if (i >= allTasks.Count)
                break;

            TaskItem task = allTasks[i];

            Console.WriteLine(
                $"{task.Id,-4} " +
                $"{task.Name,-20} " +
                $"{task.Description,-50} " +
                $"{task.Status.GetDescription(),-15} " +
                $"{task.Priority,-10}");
        }
    }

    public string EditTask()
    {
        string[] options =
        {
            "Edit Name",
            "Edit Description",
            "Change Status",
            "Change Priority",
            "Remove Task",
            "Back to Main"
        };

        Console.Clear();
        Console.Write("==== Task Edit Menu ====\n\nEnter the ID of the task you want to edit: ");

        int number;
        while (!int.TryParse(Console.ReadLine(), out number))
        {
            Console.Clear();
            Console.Write("==== Task Edit Menu ====\n\nEnter a valid number: ");
        }

        var task = _service.GetAllTasks().Find(number, (t, n) => t.Id == number);

        if (!task.HasValue)
        {
            return "MainMenu";
        }

        while (true)
        {
            int choice = SelectOption("==== Task Edit Menu ====\n\n" + task.Value, options);
            switch (choice)
            {
                case 0:
                    _service.ChangeTaskName(task.Value.Id, GetInput("Enter new task name: "));
                    continue;
                    
                case 1:
                    _service.ChangeTaskDescription(task.Value.Id, GetInput("Enter new task description: "));
                    continue;

                case 2:
                    string[] statusOptions = Enum.GetValues<TaskItem.Progress>().Select(o => o.GetDescription()).ToArray();

                    int statusChoice = SelectOption("==== Change Task Status ====", statusOptions);

                    TaskItem.Progress newStatus = (TaskItem.Progress)statusChoice;

                    _service.ChangeTaskStatus(task.Value.Id, newStatus);

                    continue;

                case 3:
                    string[] priorityOptions = Enum.GetValues<TaskItem.Importance>().Select(o => o.GetDescription()).ToArray();

                    int priorityChoice = SelectOption("==== Change Task Priority ====", priorityOptions);

                    TaskItem.Importance newPriority = (TaskItem.Importance)priorityChoice;

                    _service.ChangeTaskPriority(task.Value.Id, newPriority);

                    break;

                case 4:
                    _service.RemoveTask(task.Value.Id);
                    break;

                case 5:
                    break;;
            }

            break;
        }

        return "MainMenu";
    }

    public string GetInput(string title)
    {
        string? input;
        Console.Write(title);

        while (string.IsNullOrEmpty(input = Console.ReadLine()))
        {
            Console.Clear();
            Console.Write(title);
        }

        return input;
    }

    public string InputPassword(string title)
    {
        Console.Write(title);
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[0..^1];
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return password;
    }

    public int SelectOption(string title, string[] options)
    {
        int selectedIndex = 0;
        Console.CursorVisible = false;
        ConsoleKey key;

        while (true)
        {
            Console.Clear();
            Console.WriteLine(title + "\n");

            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("> " + options[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("  " + options[i]);
                }
            }

            key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow && selectedIndex != 0)
                selectedIndex--;

            else if (key == ConsoleKey.UpArrow)
                selectedIndex = options.Length - 1;

            else if (key == ConsoleKey.DownArrow && selectedIndex != options.Length - 1)
                selectedIndex++;

            else if (key == ConsoleKey.DownArrow)
                selectedIndex = 0;

            else if (key == ConsoleKey.Enter)
                break;

        }

        Console.CursorVisible = true;
        return selectedIndex;
    }

    public string MainMenu()
    {
        string[] options =
        {
            "View Tasks",
            "Add Task",
            "Edit Task",
            "Exit"
        };

        int choice = SelectOption("==== Main Menu ====", options);

        switch (choice)
        {
            case 0:
                Efteldingen<TaskItem> tasks = _service.GetAllTasks();
                Console.CursorVisible = false;
                int offset = 0;

                TaskFilter filter = TaskFilter.Id;

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("=== View Tasks ===\n");

                    switch (filter)
                    {
                        case TaskFilter.Id:
                            tasks.Sort((a, b) => a.Id.CompareTo(b.Id));
                            break;

                        case TaskFilter.Name:
                            tasks.Sort((a, b) => a.Name.CompareTo(b.Name));
                            break;

                        case TaskFilter.Description:
                            tasks.Sort((a, b) => a.Description.CompareTo(b.Description));
                            break;

                        case TaskFilter.Priority:
                            tasks.Sort((a, b) => a.Priority.CompareTo(b.Priority));
                            break;

                        case TaskFilter.Status:
                            tasks.Sort((a, b) => a.Status.CompareTo(b.Status));
                            break;
                    }

                    Console.WriteLine($"{"ID",-4} {"Name",-20} {"Description",-50} {"Status",-15} {"Priority",-10}");
                    Console.WriteLine(new string('-', 104) + "+");

                    DisplayTasks(10, offset);

                    Console.WriteLine(new string('-', 104) + "+");

                    Console.WriteLine("Page: ◄ " + offset / 10 + "/" + (tasks.Count - 1) / 10 + " ►");

                    Console.WriteLine($"          {new string(' ', filter.ToString().Length / 2)}        ▲");
                    Console.WriteLine($"          Sort on: {filter}");
                    Console.WriteLine($"          {new string(' ', filter.ToString().Length / 2)}        ▼");

                    Console.WriteLine("\nClick ENTER to continue...");
                    ConsoleKey key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.RightArrow && tasks.Count > offset + 10)
                        offset += 10;

                    else if (key == ConsoleKey.LeftArrow && offset >= 10)
                        offset -= 10;

                    else if (key == ConsoleKey.UpArrow && filter < TaskFilter.Status)
                        filter += 1;

                    else if (key == ConsoleKey.DownArrow && filter > TaskFilter.Id)
                        filter -= 1;

                    else if (key == ConsoleKey.Enter)
                        break;
                }
                Console.CursorVisible = true;
                return "MainMenu";

            case 1:
                Console.Clear();
                Console.WriteLine("=== Add Task ===\n");

                string name = GetInput("Enter task name: ");
                string description = GetInput("Enter task description: ");
                
                string[] priorityOptions = Enum.GetValues<TaskItem.Importance>().Select(o => o.GetDescription()).ToArray();

                int priorityChoice = SelectOption("==== Set Task Priority ====", priorityOptions);

                TaskItem.Importance priority = (TaskItem.Importance)priorityChoice;
                _service.AddTask(description, name, priority);

                return "MainMenu";

            case 2:
                return "EditTask";

            case 3:
                return "Exit";
        }

        return "Invalid";
    }

    public string StartScreen()
    {
        int choice = SelectOption("==== Login / Register ====\n", new[] { "Login", "Register" });

        if (choice == 0)
        {
            return Login();
        }
        else if (choice == 1)
        {
            return Register();
        }

        return "StartScreen";
    }

    public string Login()
    {
        Console.Clear();
        Console.WriteLine("==== Login ====\n");

        string email = GetInput("Enter your email: ");
        string password = InputPassword("Enter your password: ");
        bool isValidUser = _userService.ValidateUser(email, password);
        if (isValidUser)
        {
            loggedInUser = _userService.GetLoggedInUser(email);
            return "MainMenu";
        }

        Console.WriteLine("\nInvalid email or password.");
        Console.ReadKey();

        return "Login";
    }

    public string Register()
    {
        Console.Clear();
        Console.WriteLine("==== Register ====\n");

        string username = GetInput("Enter your username: ");
        string email = GetInput("Enter your email: ");
        string password = InputPassword("Enter your password: ");

        var user = new User(username, email, password);
        _userService.AddUser(user);

        return "MainMenu";
    }

    public void Run()
    {
        string state = "StartScreen";

        while (true)
        {
            if (state == "StartScreen")
                state = StartScreen();

            if (state == "Login")
                state = Login();

            if (state == "MainMenu")
                state = MainMenu();

            if (state == "EditTask")
                state = EditTask();

            if (state == "Exit")
                break;
        }
    }
}
