using System.ComponentModel.DataAnnotations;

namespace ComplyFlow.API.DTOs
{
    public class CreateTaskDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TaskType { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty; // Critical, High, Medium, Low
        public string Status { get; set; } = "Open"; // Default to Open
        public DateTime? DueDate { get; set; }
        public int? AssignedToUserId { get; set; }
        public int? AssignedToGroupId { get; set; }
        
        public List<CreateSubTaskDto> SubTasks { get; set; } = new List<CreateSubTaskDto>();
    }

    public class CreateSubTaskDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int? AssignedToUserId { get; set; }
        public int? AssignedToGroupId { get; set; }
    }

    public class UpdateTaskStatusDto
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
