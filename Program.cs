// ===== NAMESPACE =====
namespace TaskTracker
{
    // ===== CLASS =====
    public static class Program
    {
        // ===== FUNCTIONS =====
        public static void Main(string[] args)
        {
            TaskManager taskManager = new TaskManager();

            if (args.Length > 0)
            {
                ExecuteCommand(taskManager, args[0], string.Join(' ', args[1..]));
                return;
            }

            RunInteractiveMode(taskManager);
        }

        private static void RunInteractiveMode(TaskManager taskManager)
        {
            DisplayMessage.DisplayWelcome(taskManager.GetTaskCounts());

            while (true)
            {
                DisplayMessage.DisplaySelectOption();

                string? userInputRaw = Console.ReadLine();
                if (userInputRaw is null)
                {
                    return;
                }

                string[] userInput = userInputRaw.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                string command = userInput.Length > 0 ? userInput[0] : "";
                string arguments = userInput.Length > 1 ? userInput[1] : "";

                if (!ExecuteCommand(taskManager, command, arguments))
                {
                    return;
                }
            }
        }

        private static bool ExecuteCommand(TaskManager taskManager, string command, string arguments)
        {
            switch (command.ToLowerInvariant())
            {
                case "help":
                    DisplayMessage.DisplayHelp();
                    break;
                case "add":
                    taskManager.AddTask(arguments);
                    break;
                case "update":
                    taskManager.UpdateTask(arguments);
                    break;
                case "delete":
                    taskManager.DeleteTask(arguments);
                    break;
                case "mark-todo":
                    taskManager.MarkTask(TaskStatus.Todo, arguments);
                    break;
                case "mark-in-progress":
                    taskManager.MarkTask(TaskStatus.InProgress, arguments);
                    break;
                case "mark-done":
                    taskManager.MarkTask(TaskStatus.Done, arguments);
                    break;
                case "list":
                    taskManager.ListTasks(arguments);
                    break;
                case "exit":
                    DisplayMessage.DisplayExit();
                    return false;
                default:
                    DisplayMessage.DisplayInvalidCommand();
                    break;
            }

            return true;
        }
    }
}