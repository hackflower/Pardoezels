public class ConsoleTaskView : ITaskView
{
    private readonly ITaskService _service;
    public ConsoleTaskView(ITaskService service)
    {
        _service = service;
    }
    void DisplayTasks()
    {
        TaskItem[] allTasks = _service.GetAllTasks().ToArray();
        List<string> taskStringList = new List<string>();
        foreach(TaskItem task in allTasks)
        {
            taskStringList.Add(task.ToString());
        }
        Console.Clear();
        TaskItem ChosenTask = allTasks[ShowMenu("==== ToDo List ====\n     Select a task to edit.", taskStringList.ToArray())];
        EditTask(ChosenTask);
    }

    void EditTask(TaskItem task)
    {
        string[] options =
        {
            "Edit Name",
            "Edit Description",
            "Toggle State",
            "Remove Task",
            "Exit"
        };

            int choice = ShowMenu("==== Task Edit Menu ====", options) + 1;
            switch (choice)
            {
                case 1:
                    _service.ChangeTaskName(task.Id, Prompt("Enter new task name: "));
                    DisplayTasks();
                    break;
                case 2:
                    _service.ChangeTaskDescription(task.Id, Prompt("Enter new task description: "));
                    DisplayTasks();
                    break;
                case 3:
                    _service.ToggleTaskCompletion(task.Id);
                    DisplayTasks();
                    break;
                case 4:
                    _service.RemoveTask(task.Id);
                    DisplayTasks();
                    break;
                case 5:
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    string Prompt(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }

    int ShowMenu(string title, string[] options)
    {
        int selectedIndex = 0;
        Console.CursorVisible = false;
        ConsoleKey key;

        while (true)
        {
            Console.Clear();
            Console.WriteLine(title);
            Console.WriteLine();

            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(options[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(options[i]);
                }
            }

            key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow && selectedIndex > 0)
                selectedIndex--;

            else if (key == ConsoleKey.UpArrow && selectedIndex == 0)
                selectedIndex = options.Length - 1;

            else if (key == ConsoleKey.DownArrow && selectedIndex == options.Length - 1)
                selectedIndex = 0;

            else if (key == ConsoleKey.DownArrow && selectedIndex < options.Length - 1)
                selectedIndex++;
            
            else if (key == ConsoleKey.Enter)
            {
                Console.CursorVisible = true;
                return selectedIndex;
            }
        }
    }

    public void Run()
    {
        while (true)
        {
            string[] options =
            {
                "View Tasks",
                "Add Task",
                "Exit"
            };

            int choice = ShowMenu("==== ToDo List ====", options) + 1;
            switch (choice)
            {
                case 1:
                    DisplayTasks();
                    break;
                case 2:
                    string name = Prompt("Enter task name: ");
                    string description = Prompt("Enter task description: ");
                    string priority = Prompt("Enter task priority: " );

                    _service.AddTask(description, name, priority);
                    break;
                case 3:
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}
