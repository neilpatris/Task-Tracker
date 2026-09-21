// ===== USING =====
using System.Text.Json;
using System.Text.Json.Serialization;
// ===== NAMESPACE =====
namespace TaskTracker
{
    // ===== CLASS =====
    public class TaskManager
    {
        // ===== FIELDS =====
        private const string FileName = "tasks.json";
        private const string DirectoryName = "JsonData";

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.KebabCaseLower)
            }
        };
        
        private static readonly Dictionary<string, TaskStatus> _statusByName = new Dictionary<string, TaskStatus>(StringComparer.OrdinalIgnoreCase)
        {
            { "todo", TaskStatus.Todo },
            { "in-progress", TaskStatus.InProgress },
            { "done", TaskStatus.Done }
        };

        private List<Task> _tasks = new List<Task>();
        private bool _isFileContentKnown = true;
        // ===== FUNCTIONS =====
        public TaskManager()
        {
            LoadTasksFromFile();
        }

        public void AddTask(string arguments)
        {
            string description = Normalize(arguments);

            if (string.IsNullOrWhiteSpace(description))
            {
                DisplayMessage.DisplayInvalidDescription();
                return;
            }

            Task newTask = new Task(description, TaskStatus.Todo);
            _tasks.Add(newTask);
            SaveTasksToFile();
            DisplayMessage.DisplayAddTaskSuccess(newTask);
        }

        public void UpdateTask(string arguments)
        {
            string[] argumentParts = arguments.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

            if (argumentParts.Length < 2)
            {
                DisplayMessage.DisplayInvalidDescription();
                return;
            }

            string description = Normalize(argumentParts[1]);

            if (string.IsNullOrWhiteSpace(description))
            {
                DisplayMessage.DisplayInvalidDescription();
                return;
            }

            if (!TryResolveTask(argumentParts[0], out Task taskToUpdate))
            {
                return;
            }

            taskToUpdate.UpdateTask(description);
            SaveTasksToFile();
            DisplayMessage.DisplayUpdateTaskSuccess(taskToUpdate);
        }

        public void DeleteTask(string arguments)
        {
            if (!TryResolveTask(arguments, out Task taskToDelete))
            {
                return;
            }

            _tasks.Remove(taskToDelete);
            SaveTasksToFile();
            DisplayMessage.DisplayDeleteTaskSuccess(taskToDelete.Id);
        }

        public void MarkTask(TaskStatus status, string arguments)
        {
            if (!TryResolveTask(arguments, out Task taskToMark))
            {
                return;
            }

            taskToMark.Mark(status);
            SaveTasksToFile();
            DisplayMessage.DisplayMarkTaskSuccess(taskToMark);
        }

        public Dictionary<TaskStatus, int> GetTaskCounts()
        {
            Dictionary<TaskStatus, int> taskCounts = new Dictionary<TaskStatus, int>
            {
                { TaskStatus.Todo, _tasks.Count(t => t.Status == TaskStatus.Todo) },
                { TaskStatus.InProgress, _tasks.Count(t => t.Status == TaskStatus.InProgress) },
                { TaskStatus.Done, _tasks.Count(t => t.Status == TaskStatus.Done) }
            };

            return taskCounts;
        }

        public void ListTasks(string arguments)
        {
            string statusFilter = Normalize(arguments);

            if (string.IsNullOrWhiteSpace(statusFilter))
            {
                DisplayTasks(_tasks, "Here are all the tasks:");
                return;
            }

            if (!_statusByName.TryGetValue(statusFilter, out TaskStatus status))
            {
                DisplayMessage.DisplayInvalidStatus(_statusByName.Keys);
                return;
            }

            List<Task> filteredTasks = _tasks.Where(t => t.Status == status).ToList();
            DisplayTasks(filteredTasks, $"Here are the tasks with status '{statusFilter}':");
        }

        private void DisplayTasks(List<Task> tasks, string title)
        {
            if (tasks.Count == 0)
            {
                DisplayMessage.DisplayNoTasks();
                return;
            }

            DisplayMessage.DisplayTaskList(tasks, title);
        }

        private void SaveTasksToFile()
        {
            if (!_isFileContentKnown)
            {
                DisplayMessage.DisplaySaveBlocked(FileName);
                return;
            }

            try
            {
                string path = GetFilePath();
                string json = JsonSerializer.Serialize(_tasks, _jsonOptions);
                File.WriteAllText(path, json);
            }
            catch (UnauthorizedAccessException exception)
            {
                DisplayMessage.DisplayFileAccessDenied(exception.Message);
            }
            catch (IOException exception)
            {
                DisplayMessage.DisplayFileWriteError(exception.Message);
            }
        }

        private void LoadTasksFromFile()
        {
            try
            {
                string path = GetFilePath();
                if (!File.Exists(path))
                {
                    return;
                }

                string json = File.ReadAllText(path);
                _tasks = JsonSerializer.Deserialize<List<Task>>(json, _jsonOptions) ?? new List<Task>();
            }
            catch (UnauthorizedAccessException exception)
            {
                _isFileContentKnown = false;
                DisplayMessage.DisplayFileAccessDenied(exception.Message);
                return;
            }
            catch (IOException exception)
            {
                _isFileContentKnown = false;
                DisplayMessage.DisplayFileReadError(exception.Message);
                return;
            }
            catch (JsonException exception)
            {
                _isFileContentKnown = false;
                DisplayMessage.DisplayFileCorrupted(exception.Message);
                return;
            }

            if (_tasks.Count > 0)
            {
                Task.SyncCounter(_tasks.Max(t => t.Id));
            }
        }

        private Task? FindTaskById(int taskId)
        {
            return _tasks.FirstOrDefault(t => t.Id == taskId);
        }

        private bool TryResolveTask(string rawId, out Task resolvedTask)
        {
            resolvedTask = null!;

            if (!int.TryParse(rawId, out int taskId))
            {
                DisplayMessage.DisplayInvalidId();
                return false;
            }

            Task? task = FindTaskById(taskId);
            if (task is null)
            {
                DisplayMessage.DisplayTaskNotFound(taskId);
                return false;
            }

            resolvedTask = task;
            return true;
        }

        private string GetFilePath()
        {
            Directory.CreateDirectory(DirectoryName);
            return Path.Combine(DirectoryName, FileName);
        }

        private string Normalize(string raw)
        {
            return raw.Trim().Trim('"').Trim();
        }
    }
}
