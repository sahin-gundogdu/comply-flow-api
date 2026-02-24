namespace ComplyFlow.API.DTOs
{
    public class SubTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        
        public int? AssignedToUserId { get; set; }
        public string? AssignedToUserName { get; set; }

        public int? AssignedToGroupId { get; set; }
        public string? AssignedToGroupName { get; set; }
    }
}
