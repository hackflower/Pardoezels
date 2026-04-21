public class ConsoleTaskView : ITaskView
{
    public User? loggedInUser { get; set; }
    private readonly ITaskService _taskservice;
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
        _taskservice = taskService;
        _userService = userService;
    }

    public void DisplayTasks(int amount, int offset = 0)
    {
        IMyCollection<TaskItem> allTasks = _taskservice.GetAllTasks();

        for (int i = offset; i < amount + offset; i++)
        {
            if (i >= allTasks.Count) break;

            TaskItem task = allTasks.ToArray()[i];

            Console.WriteLine(
                $"{task.Id,-4} " +
                $"{task.Name,-30} " +
                $"{task.Description,-65} " +
                $"{task.Status.GetDescription(),-15} " +
                $"{task.Priority,-10} " +
                $"{string.Join(", ", task.AssignedUsersIds.Select(u => _userService.GetUserById(u)?.Username ?? "Unknown")),-30}" +
                $"{string.Join(", ", task.DependenciesIds.Select(t => _taskservice.GetTaskById(t)?.Name ?? "Unknown")),-20}"
            );
        }
    }

    public void PressToContinue(string message)
    {
        Console.Clear();
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("\nPress enter to continue...");
        Console.ReadKey();
        Console.ResetColor();
    }

    public string EditTask()
    {
        string[] options =
        {
            "Edit Name",
            "Edit Description",
            "Edit Status",
            "Edit Priority",
            "Add dependency",
            "Remove dependency",
            "Add Assigned User",
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

        var task = _taskservice.GetAllTasks().FindBy(number, (t, n) => t.Id.CompareTo(number));
        if (!task.HasValue) return "MainMenu";

        if (loggedInUser == null || !task.Value.AssignedUsersIds.FindBy(loggedInUser, (t, u) => t.CompareTo(u.Id)).HasValue)
        {
            PressToContinue("\nYou can only edit tasks assigned to you. Press any key to return to the main menu...");
            return "MainMenu";
        }

        while (true)
        {
            int choice = SelectOption("==== Task Edit Menu ====\n\n" + task.Value, options);
            switch (choice)
            {
                case 0:
                    _taskservice.ChangeTaskName(task.Value.Id, GetInput("Enter new task name: "));
                    continue;
                    
                case 1:
                    _taskservice.ChangeTaskDescription(task.Value.Id, GetInput("Enter new task description: "));
                    continue;

                case 2:
                    string[] statusOptions = Enum.GetValues<TaskItem.Progress>().Select(o => o.GetDescription()).ToArray();

                    bool allDone = true;
                    for (int i = 0; i < task.Value.DependenciesIds.Count; i++)
                        if (_taskservice.GetTaskById(task.Value.DependenciesIds.ToArray()[i]).Status != TaskItem.Progress.Completed) allDone = false;

                    if (!allDone)
                    {
                        PressToContinue("Dependent tasks must be completed first.");
                        continue;
                    }

                    int statusChoice = SelectOption("==== Change Task Status ====", statusOptions);

                    TaskItem.Progress newStatus = (TaskItem.Progress)statusChoice;

                    _taskservice.ChangeTaskStatus(task.Value.Id, newStatus);

                    continue;

                case 3:
                    string[] priorityOptions = Enum.GetValues<TaskItem.Importance>().Select(o => o.GetDescription()).ToArray();

                    int priorityChoice = SelectOption("==== Change Task Priority ====", priorityOptions);

                    TaskItem.Importance newPriority = (TaskItem.Importance)priorityChoice;

                    _taskservice.ChangeTaskPriority(task.Value.Id, newPriority);

                    break;

                case 4:
                    Console.Clear();
                    Console.Write("==== Task Edit Menu ====\n\nEnter the ID of the task you want to add as dependency: ");

                    int id;
                    while (!int.TryParse(Console.ReadLine(), out id))
                    {
                        Console.Clear();
                        Console.Write("==== Task Edit Menu ====\n\nEnter a valid number: ");
                    }

                    _taskservice.AddDependency(id, task.Value);
                    break;

                case 5:
                    Console.Clear();
                    Console.Write("==== Task Edit Menu ====\n\nEnter the ID of the task you want to remove from dependencies: ");

                    int idOfTask;
                    while (!int.TryParse(Console.ReadLine(), out idOfTask))
                    {
                        Console.Clear();
                        Console.Write("==== Task Edit Menu ====\n\nEnter a valid number: ");
                    }

                    var taskToRemove = _taskservice.GetAllTasks().FindBy(idOfTask, (t, n) => t.Id.CompareTo(idOfTask));
                    if (taskToRemove.HasValue) task.Value.DependenciesIds.Remove(taskToRemove.Value.Id);
                    break;

                case 6:
                    if (_userService.GetAllUsers().Count == 0)
                    {
                        PressToContinue("\nNo users available to assign. Press any key to continue...");
                        break;
                    }

                    if (task.Value.AssignedUsersIds.Count >= 3)
                    {
                        PressToContinue("\nThis task has reached the maximum number of assigned users. Press any key to continue...");
                        break;
                    }


                    string[] userAssignmentOptions = _userService.GetAllUsers().Select(u => u.Username).ToArray();

                    int userChoice = SelectOption("==== Add Assigned User ====", userAssignmentOptions);

                    User newAssignedUser = _userService.GetAllUsers().FindBy(userAssignmentOptions[userChoice], (u, username) => u.Username == username ? 0 : -1).Value;

                    if (task.Value.AssignedUsersIds.FindBy(newAssignedUser.Id, (u, id) => u.CompareTo(id)).HasValue)
                    {
                        PressToContinue("\nThis user is already assigned to the task. Press any key to continue...");
                        break;
                    }

                    _taskservice.AddAssignedUser(task.Value.Id, newAssignedUser);

                    break;

                case 7:
                    _taskservice.RemoveTask(task.Value.Id);
                    break;

                case 8:
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

    public static int SelectOption(string title, string[] options)
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

        int choice = SelectOption($"==== Main Menu ====\n\nLogged in as: {loggedInUser!.Username}", options);

        switch (choice)
        {
            case 0:
                IMyCollection<TaskItem> tasks = _taskservice.GetAllTasks();
                Console.CursorVisible = false;

                int offset = 0;
                bool orderAsc = false;

                TaskFilter filter = TaskFilter.Id;

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("=== View Tasks ===\n");

                    switch (filter)
                    {
                        case TaskFilter.Id:
                            if (orderAsc) tasks.Sort((b, a) => a.Id.CompareTo(b.Id));
                            else tasks.Sort((a, b) => a.Id.CompareTo(b.Id));
                            break;

                        case TaskFilter.Name:
                            if (orderAsc) tasks.Sort((b, a) => a.Name.CompareTo(b.Name));
                            else tasks.Sort((a, b) => a.Name.CompareTo(b.Name));
                            break;

                        case TaskFilter.Description:
                            if (orderAsc) tasks.Sort((b, a) => a.Description.CompareTo(b.Description));
                            else tasks.Sort((a, b) => a.Description.CompareTo(b.Description));
                            break;

                        case TaskFilter.Priority:
                            if (orderAsc) tasks.Sort((b, a) => a.Priority.CompareTo(b.Priority));
                            else tasks.Sort((a, b) => a.Priority.CompareTo(b.Priority));
                            break;

                        case TaskFilter.Status:
                            if (orderAsc) tasks.Sort((b, a) => a.Status.CompareTo(b.Status));
                            else tasks.Sort((a, b) => a.Status.CompareTo(b.Status));
                            break;
                    }

                    Console.WriteLine($"{"ID",-4} {"Name",-30} {"Description",-65} {"Status",-15} {"Priority",-10} {"Assigned Users",-29} {"Dependencies",-20}");
                    Console.WriteLine(new string('-', 186) + "+");

                    DisplayTasks(10, offset);

                    Console.WriteLine(new string('-', 186) + "+");

                    Console.WriteLine("Page: ◄ " + offset / 10 + "/" + (tasks.Count - 1) / 10 + " ►");

                    Console.WriteLine($"          {new string(' ', filter.ToString().Length / 2)}        ▲");
                    Console.WriteLine($"          Sort on: {filter} {(orderAsc ? "(DESC)" : "(ASC)")}");
                    Console.WriteLine($"          {new string(' ', filter.ToString().Length / 2)}        ▼");
                    Console.WriteLine("\nPress TAB to switch sorting order...");
                    Console.WriteLine("\nClick ENTER to continue...");
                    ConsoleKey key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.RightArrow && tasks.Count > offset + 10) offset += 10;
                    else if (key == ConsoleKey.LeftArrow && offset >= 10) offset -= 10;
                    else if (key == ConsoleKey.UpArrow && filter < TaskFilter.Status) filter += 1;
                    else if (key == ConsoleKey.DownArrow && filter > TaskFilter.Id) filter -= 1;
                    else if (key == ConsoleKey.Tab) orderAsc = !orderAsc;
                    else if (key == ConsoleKey.Enter) break;
                }
                Console.CursorVisible = true;
                return "MainMenu";

            case 1:
                Console.Clear();
                Console.WriteLine("=== Add Task ===\n");

                string name = GetInput("Enter task name: ");
                string description = GetInput("Enter task description: ");
                
                string[] priorityOptions = Enum.GetValues<TaskItem.Importance>().Select(o => o.GetDescription()).ToArray();
                string[] userAssignmentOptions = _userService.GetAllUsers().Select(u => u.Username).ToArray();

                int priorityChoice = SelectOption("==== Set Task Priority ====", priorityOptions);

                User assignedUser = _userService.GetAllUsers().FindBy(userAssignmentOptions[SelectOption("==== Assign User to Task ====", userAssignmentOptions)], (u, username) => u.Username == username ? 0 : -1).Value;
                TaskItem.Importance priority = (TaskItem.Importance)priorityChoice;

                _taskservice.AddTask(name, description, priority, assignedUser.Id);

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
        int choice = SelectOption("==== Login / Register ====", new[] { "Login", "Register" });

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
            loggedInUser = _userService.GetUserByEmail(email);
            return "MainMenu";
        }

        PressToContinue("\nInvalid email or password.");
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

        if (!_userService.UserValid(user))
        {
            PressToContinue("\nInvalid user details. Make sure your email is valid and not already in use.");
            return "Register";
        }

        _userService.AddUser(user);
        loggedInUser = _userService.GetUserByEmail(email);

        return "MainMenu";
    }

    public void Run()
    {
        string state = "StartScreen";

        while (true)
        {
            if (state == "StartScreen")
                state = StartScreen();

            if (state == "Register")
                state = Register();

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
