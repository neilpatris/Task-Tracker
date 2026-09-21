// ===== NAMESPACE =====
namespace TaskTracker
{
    // ===== CLASS =====
    public class Task : ITaskProperties
    {
        // ===== FIELDS =====
        private static int _idCounter = 0;

        public int Id { get; init; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // ===== FUNCTIONS =====
        public Task(string description, TaskStatus status)
        {
            Id = ++_idCounter;
            Description = description;
            Status = status;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public void UpdateTask(string description)
        {
            Description = description;
            UpdatedAt = DateTime.Now;
        }

        public void Mark(TaskStatus status)
        {
            Status = status;
            UpdatedAt = DateTime.Now;
        }

        public static void SyncCounter(int lastId)
        {
            _idCounter = lastId;
        }
    }
}