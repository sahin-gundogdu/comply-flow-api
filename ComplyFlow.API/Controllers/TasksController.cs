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
                .Include(t => t.SubTasks)
                    .ThenInclude(s => s.AssignedToUser)
                .Include(t => t.SubTasks)
                    .ThenInclude(s => s.AssignedToGroup)
                .ToListAsync();

            var taskDtos = tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                Priority = t.Priority,
                TaskType = t.TaskType,
                DueDate = t.DueDate,
                AssignedToUserId = t.AssignedToUserId,
                AssignedToUserName = t.AssignedToUser?.FullName,
                AssignedToGroupId = t.AssignedToGroupId,
                AssignedToGroupName = t.AssignedToGroup?.Name,
                SubTasks = t.SubTasks.Select(s => new SubTaskDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    AssignedToUserId = s.AssignedToUserId,
                    AssignedToUserName = s.AssignedToUser?.FullName,
                    AssignedToGroupId = s.AssignedToGroupId,
                    AssignedToGroupName = s.AssignedToGroup?.Name
                }).ToList()
            }).ToList();

            return Ok(taskDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _context.TaskItems
                .Include(t => t.AssignedToUser)
                .Include(t => t.AssignedToGroup)
                .Include(t => t.SubTasks)
                    .ThenInclude(s => s.AssignedToUser)
                .Include(t => t.SubTasks)
                    .ThenInclude(s => s.AssignedToGroup)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            var taskDto = new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                TaskType = task.TaskType,
                DueDate = task.DueDate,
                AssignedToUserId = task.AssignedToUserId,
                AssignedToUserName = task.AssignedToUser?.FullName,
                AssignedToGroupId = task.AssignedToGroupId,
                AssignedToGroupName = task.AssignedToGroup?.Name,
                SubTasks = task.SubTasks.Select(s => new SubTaskDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    AssignedToUserId = s.AssignedToUserId,
                    AssignedToUserName = s.AssignedToUser?.FullName,
                    AssignedToGroupId = s.AssignedToGroupId,
                    AssignedToGroupName = s.AssignedToGroup?.Name
                }).ToList()
            };

            return Ok(taskDto);
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
                        AssignedToUserId = subDto.AssignedToUserId,
                        AssignedToGroupId = subDto.AssignedToGroupId
                    });
                }
            }

            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();

            var taskDto = new TaskDto
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                Status = taskItem.Status,
                Priority = taskItem.Priority,
                TaskType = taskItem.TaskType,
                DueDate = taskItem.DueDate,
                AssignedToUserId = taskItem.AssignedToUserId,
                AssignedToGroupId = taskItem.AssignedToGroupId,
                SubTasks = taskItem.SubTasks.Select(s => new SubTaskDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    AssignedToUserId = s.AssignedToUserId,
                    AssignedToGroupId = s.AssignedToGroupId
                }).ToList()
            };

            return CreatedAtAction(nameof(GetTaskById), new { id = taskItem.Id }, taskDto);
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
