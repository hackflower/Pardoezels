public class ConsoleTaskView : ITaskView
{
    private readonly ITaskService _service;

    public ConsoleTaskView(ITaskService service)
    {
        _service = service;
    }

    public void DisplayTasks(int amount, int offset = 0)
    {
        Efteldingen<TaskItem> allTasks = _service.GetAllTasks();

        for (int i = offset; i < amount + offset; i++)
        {
            if (i >= allTasks.Count)
                break;

            Console.WriteLine(allTasks[i]);
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
                    _service.ChangeTaskName(task.Value.Id, Prompt("Enter new task name: "));
                    continue;
                    
                case 1:
                    _service.ChangeTaskDescription(task.Value.Id, Prompt("Enter new task description: "));
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

    public string Prompt(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
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
                Console.CursorVisible = false;
                int offset = 0;

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("=== View Tasks ===\n");
                    DisplayTasks(5, offset);

                    Console.WriteLine("Page: (" + offset / 5 + "/" + _service.GetAllTasks().Count / 5 + ")");

                    Console.WriteLine("\nUse arrows to navigate");

                    Console.WriteLine("\nClick ENTER to continue...");
                    ConsoleKey key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.RightArrow && _service.GetAllTasks().Count > offset + 5)
                        offset += 5;

                    if (key == ConsoleKey.LeftArrow && offset >= 5)
                        offset -= 5;

                    if (key == ConsoleKey.Enter)
                        break;
                }
                Console.CursorVisible = true;
                return "MainMenu";

            case 1:
                Console.Clear();
                Console.WriteLine("=== Add Task ===\n");

                string name = Prompt("Enter task name: ");
                string description = Prompt("Enter task description: ");
                
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

    public void Run()
    {
        string state = "MainMenu";

        while (true)
        {
            if (state == "MainMenu")
                state = MainMenu();

            if (state == "EditTask")
                state = EditTask();

            if (state == "Exit")
                break;
        }
    }
}
