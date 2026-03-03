public class ConsoleTaskView : ITaskView
{
    private readonly ITaskService _service;
    public ConsoleTaskView(ITaskService service)
    {
        _service = service;
    }
    void DisplayTasks(IEnumerable<TaskItem> tasks)
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
                    break;
                case 2:
                    _service.ChangeTaskDescription(task.Id, Prompt("Enter new task description: "));
                    break;
                case 3:
                    _service.ToggleTaskCompletion(task.Id);
                    break;
                case 4:
                    _service.RemoveTask(task.Id);
                    break;
                case 5:
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
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
        ConsoleKey key;

        do
        {
            Console.Clear();
            Console.WriteLine(title);
            Console.WriteLine();

            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("" + options[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("" + options[i]);
                }
            }

            key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow && selectedIndex > 0)
                selectedIndex--;

            if (key == ConsoleKey.DownArrow && selectedIndex < options.Length - 1)
                selectedIndex++;

        } while (key != ConsoleKey.Enter);

        return selectedIndex;
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
                    DisplayTasks(_service.GetAllTasks());
                    break;
                case 2:
                    string name = Prompt("Enter task name: ");
                    string description = Prompt("Enter task description: ");

                    _service.AddTask(description, name);
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
