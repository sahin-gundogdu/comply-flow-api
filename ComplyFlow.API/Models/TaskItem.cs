using System;

namespace ComplyFlow.API.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        
        public int? AssignedToUserId { get; set; }
        public User? AssignedToUser { get; set; }

        public int? AssignedToGroupId { get; set; }
        public Group? AssignedToGroup { get; set; }
    }
}
