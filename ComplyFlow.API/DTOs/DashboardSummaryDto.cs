using System.Collections.Generic;

namespace ComplyFlow.API.DTOs
{
    public class DashboardSummaryDto
    {
        public int TotalTasks { get; set; }
        public int PendingTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int OverdueTasks { get; set; }
        public List<TaskDto> RecentTasks { get; set; } = new List<TaskDto>();
    }
}
