using ComplyFlow.API.Data;
using ComplyFlow.API.DTOs;
using ComplyFlow.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;

namespace ComplyFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GroupsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GroupsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroups()
        {
            var groups = await _context.Groups
                .Select(g => new GroupDto
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToListAsync();

            return Ok(groups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupById(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            var groupDto = new GroupDto
            {
                Id = group.Id,
                Name = group.Name
            };

            return Ok(groupDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Groups.AnyAsync(g => g.Name == dto.Name))
            {
                return BadRequest("Bu grup adı zaten kullanılıyor.");
            }

            var group = new Group
            {
                Name = dto.Name
            };

            try
            {
                _context.Groups.Add(group);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }

            var groupDto = new GroupDto
            {
                Id = group.Id,
                Name = group.Name
            };

            return CreatedAtAction(nameof(GetGroupById), new { id = group.Id }, groupDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup(int id, [FromBody] UpdateGroupDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            if (group.Name != dto.Name && await _context.Groups.AnyAsync(g => g.Name == dto.Name))
            {
                return BadRequest("Bu grup adı zaten kullanılıyor.");
            }

            group.Name = dto.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            try
            {
                var groupTasks = await _context.TaskItems.Where(t => t.AssignedToGroupId == id).ToListAsync();
                foreach (var task in groupTasks)
                {
                    task.AssignedToGroupId = null;
                }

                var groupSubTasks = await _context.SubTasks.Where(s => s.AssignedToGroupId == id).ToListAsync();
                foreach (var subTask in groupSubTasks)
                {
                    subTask.AssignedToGroupId = null;
                }

                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }

            return NoContent();
        }
    }
}
