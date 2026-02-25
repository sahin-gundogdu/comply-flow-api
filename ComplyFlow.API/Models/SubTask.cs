namespace ComplyFlow.API.Models
{
    public class SubTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        
        public int? AssignedToUserId { get; set; }
        public User? AssignedToUser { get; set; }

        public int? AssignedToGroupId { get; set; }
        public Group? AssignedToGroup { get; set; }

        public int TaskItemId { get; set; }
        public TaskItem? TaskItem { get; set; }
    }
}
