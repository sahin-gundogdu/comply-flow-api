namespace ComplyFlow.API.DTOs
{
    public class SubTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        
        public int? AssignedToUserId { get; set; }
        public string? AssignedToUserName { get; set; }

        public int? AssignedToGroupId { get; set; }
        public string? AssignedToGroupName { get; set; }
    }
}
