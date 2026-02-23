using System;
using System.ComponentModel.DataAnnotations;

namespace ComplyFlow.API.DTOs
{
    public class UpdateTaskDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string TaskType { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public int? AssignedToUserId { get; set; }
        public int? AssignedToGroupId { get; set; }
    }
}
