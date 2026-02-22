using ComplyFlow.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComplyFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _context.TaskItems
                .Include(t => t.AssignedToUser)
                .Include(t => t.AssignedToGroup)
                .ToListAsync();

            return Ok(tasks);
        }
    }
}
