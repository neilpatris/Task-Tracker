// ===== NAMESPACE =====
namespace TaskTracker
{
    // ===== CLASS =====
    public static class DisplayMessage
    {
        // ===== FUNCTIONS =====
        // global messages 
        public static void DisplayWelcome(Dictionary<TaskStatus, int> taskCounts)
        {
            Console.WriteLine("Welcome to the Task Tracker!");
            Console.WriteLine($"You have {taskCounts[TaskStatus.Todo]} tasks to do, {taskCounts[TaskStatus.InProgress]} tasks in progress, and {taskCounts[TaskStatus.Done]} done tasks.");
            Console.WriteLine("Type 'help' to see the list of commands.");
        }
        
        public static void DisplayHelp()
        {
            (string Command, string Description)[] commands =
            {
                ("add \"Task description\"",                  "Add a new task with the description."),
                ("update <task_id> \"New task description\"", "Update the description of the task."),
                ("delete <task_id>",                          "Delete the task with the ID."),
                ("mark-todo <task_id>",                       "Mark the task as 'Todo'."),
                ("mark-in-progress <task_id>",                "Mark the task as 'In Progress'."),
                ("mark-done <task_id>",                       "Mark the task as 'Done'."),
                ("list",                                      "List all tasks."),
                ("list \"status\"",                           "List tasks filtered by status (todo, in-progress, done)."),
                ("exit",                                      "Exit the application."),
            };

            Console.WriteLine("Here are the list of commands:");
            foreach ((string command, string description) in commands)
            {
                Console.WriteLine($"\t{command,-50}{description}");
            }
        }

        public static void DisplayTaskList(List<Task> tasks, string title)
        {
            Console.WriteLine(title);

            DisplayTableHeader();
            foreach (Task task in tasks)
            {
                DisplayTaskRow(task);
            }
        }

        public static void DisplaySelectOption()
        {
            Console.WriteLine("Please select an option:");
        }

        public static void DisplayExit()
        {
            Console.WriteLine("Exiting the application...");
        }

        // Success messages
        public static void DisplayAddTaskSuccess(Task task)
        {
            Console.WriteLine($"Task added successfully! (ID: {task.Id})");
        }

        public static void DisplayUpdateTaskSuccess(Task task)
        {
            Console.WriteLine($"Task updated successfully! (ID: {task.Id})");
        }

        public static void DisplayDeleteTaskSuccess(int taskId)
        {
            Console.WriteLine($"Task deleted successfully! (ID: {taskId})");
        }

        public static void DisplayMarkTaskSuccess(Task task)
        {
            Console.WriteLine($"Task marked as {task.Status} successfully! (ID: {task.Id})");
        }

        // Error messages
        public static void DisplayInvalidCommand()
        {
            Console.WriteLine("Invalid command. Type 'help' to see the list of commands.");
        }

        public static void DisplayInvalidId()
        {
            Console.WriteLine("Invalid task ID. Please provide a valid integer.");
        }

        public static void DisplayInvalidDescription()
        {
            Console.WriteLine("Invalid task description. Please provide a non-empty description.");
        }

        public static void DisplayTaskNotFound(int taskId)
        {
            Console.WriteLine($"Task with ID {taskId} not found.");
        }

        public static void DisplayNoTasks()
        {
            Console.WriteLine("No tasks found.");
        }

        public static void DisplayInvalidStatus(IEnumerable<string> allowedStatuses)
        {
            Console.WriteLine($"Invalid status. Please use: {string.Join(", ", allowedStatuses)}.");
        }

        // File error messages
        public static void DisplayFileReadError(string errorMessage)
        {
            Console.WriteLine($"Error reading the file: {errorMessage}");
        }

        public static void DisplayFileWriteError(string errorMessage)
        {
            Console.WriteLine($"Error writing the file: {errorMessage}");
        }

        public static void DisplayFileAccessDenied(string errorMessage)
        {
            Console.WriteLine($"Access to the file denied: {errorMessage}");
        }

        public static void DisplayFileCorrupted(string errorMessage)
        {
            Console.WriteLine($"The backup file is not a JSON file or is corrupted: {errorMessage}");
        }

        public static void DisplaySaveBlocked(string fileName)
        {
            Console.WriteLine($"Nothing was saved: '{fileName}' could not be read at startup.");
            Console.WriteLine("Fix the file or delete it, then run the command again.");
        }

        private static string FormatDate(DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy HH:mm");
        }

        private static string Truncate(string text, int maxLength)
        {
            if (text.Length <= maxLength)
            {
                return text;
            }

            return text.Substring(0, maxLength) + "…";
        }

        private static void DisplayTableHeader()
        {
            Console.WriteLine($"{"ID",-6}{"DESCRIPTION",-40}{"STATUS",-14}{"CREATED",-20}{"UPDATED",-20}");
            Console.WriteLine(new string('=', 100));
        }

        private static void DisplayTaskRow(Task task)
        {
            Console.WriteLine($"{task.Id,-6}{Truncate(task.Description, 38),-40}{task.Status,-14}{FormatDate(task.CreatedAt),-20}{FormatDate(task.UpdatedAt),-20}");
        }
    }   
}
