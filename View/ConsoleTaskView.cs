public class ConsoleTaskView : ITaskView
{
    private readonly ITaskService _service;
    public ConsoleTaskView(ITaskService service)
    {
        _service = service;
    }
    void DisplayTasks(IEnumerable<TaskItem> tasks)
    {
        Console.Clear();
        Console.WriteLine("==== ToDo List ====");
        foreach (var task in tasks)
            Console.WriteLine($"{task}");
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
                    Console.WriteLine("> " + options[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("  " + options[i]);
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

    public static string? LiveChangeInput(string toBeChanged)
    {
        string newValue = "";
        do
        {
            Console.Clear();
            Console.WriteLine($"fill in new {toBeChanged}");
            Console.WriteLine(newValue);
            ConsoleKeyInfo pressedKey = Console.ReadKey(true);
            switch (pressedKey.Key)
            {
                case ConsoleKey.Backspace:
                    if (newValue.Length > 0)
                    {
                        newValue = newValue.Remove(newValue.Length - 1);
                    }
                    break;
                case ConsoleKey.Enter:
                    return newValue;
                case ConsoleKey.Escape:
                    return null;
                default:
                    newValue += pressedKey.KeyChar;
                    break;
            }

        } while (true);

    }

    void AddTaskForm()
    {
        string q1 = "name:";
        string q2 = "description ";
        string q3 = "priority";

        string[] questions = [q1, q2, q3];
        int selectedQuestion = ShowMenu("personalData", questions);
        int selectedQuestionIndex = Array.IndexOf(questions, selectedQuestion);

        switch (selectedQuestionIndex)
        {
            case 0://name
                string? newName = LiveChangeInput("name");
                break;

            case 1://description
                string? newDescription = LiveChangeInput("description");
                break;
            case 2://priority
                string? newPriority = LiveChangeInput("priority");
                break;
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
                "Remove Task",
                "Toggle Task State",
                "Exit"
            };

            int choice = ShowMenu("==== ToDo List ====", options) + 1;
            switch (choice)
            {
                case 1:
                    DisplayTasks(_service.GetAllTasks());
                    Prompt("Press enter to return to main menu...");
                    break;
                case 2:
                    string[] addTaskOptions =
                    {
                        "Enter task name",
                        "Enter task description:",
                        "Enter task priority:",
                        "[CANCEL]",
                        "[ADD]"
                    };
                    int addTaskChoice = ShowMenu("==== Add Task ====", addTaskOptions) + 1;
                    //_service.AddTask(description, name, priority);
                    break;
                case 3:
                    string removeIdStr = Prompt("Enter task id to remove: ");
                    if (int.TryParse(removeIdStr, out int removeId))
                    {
                        _service.RemoveTask(removeId);
                    }
                    break;
                case 4:
                    string toggleIdStr = Prompt("Enter task id to toggle: ");
                    if (int.TryParse(toggleIdStr, out int toggleId))
                    {
                        _service.ToggleTaskCompletion(toggleId);
                    }
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
}
// test
