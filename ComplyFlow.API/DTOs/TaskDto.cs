using System;
using System.Collections.Generic;

namespace ComplyFlow.API.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string TaskType { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        
        public int? AssignedToUserId { get; set; }
        public string? AssignedToUserName { get; set; }

        public int? AssignedToGroupId { get; set; }
        public string? AssignedToGroupName { get; set; }

        public List<SubTaskDto> SubTasks { get; set; } = new List<SubTaskDto>();
    }
}
