using ComplyFlow.API.Data;
using ComplyFlow.API.DTOs;
using ComplyFlow.API.Models;
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

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskItem = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                TaskType = dto.TaskType,
                Priority = dto.Priority,
                Status = dto.Status ?? "Open",
                DueDate = dto.DueDate,
                AssignedToUserId = dto.AssignedToUserId,
                AssignedToGroupId = dto.AssignedToGroupId
            };

            if (dto.SubTasks != null && dto.SubTasks.Any())
            {
                foreach (var subDto in dto.SubTasks)
                {
                    taskItem.SubTasks.Add(new SubTask
                    {
                        Title = subDto.Title,
                        AssignedToUserId = subDto.AssignedToUserId
                    });
                }
            }

            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTasks), new { id = taskItem.Id }, taskItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Status = dto.Status;
            task.Priority = dto.Priority;
            task.TaskType = dto.TaskType;
            task.DueDate = dto.DueDate;
            task.AssignedToUserId = dto.AssignedToUserId;
            task.AssignedToGroupId = dto.AssignedToGroupId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
