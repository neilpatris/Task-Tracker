// ===== NAMESPACE =====
namespace TaskTracker
{
    // ===== INTERFACE =====
    public interface ITaskProperties
    {
        int Id { get; }
        string Description { get; set; }
        TaskStatus Status { get; set; }
        DateTime CreatedAt { get; }
        DateTime UpdatedAt { get; }

        void UpdateTask(string description);
        void Mark(TaskStatus status);
    }
}