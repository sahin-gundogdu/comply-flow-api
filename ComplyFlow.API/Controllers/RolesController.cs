using ComplyFlow.API.Data;
using ComplyFlow.API.DTOs;
using ComplyFlow.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComplyFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RolesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.Roles
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToListAsync();

            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Roles.AnyAsync(r => r.Name == dto.Name))
            {
                return BadRequest("Bu rol adı zaten kullanılıyor.");
            }

            var role = new Role
            {
                Name = dto.Name
            };

            try
            {
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }

            var roleDto = new RoleDto
            {
                Id = role.Id,
                Name = role.Name
            };

            return CreatedAtAction(nameof(GetRoles), new { id = role.Id }, roleDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            if (role.Name != dto.Name && await _context.Roles.AnyAsync(r => r.Name == dto.Name))
            {
                return BadRequest("Bu rol adı zaten kullanılıyor.");
            }

            role.Name = dto.Name;

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
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            // Note: Currently we are just deleting the role. 
            // In a real scenario we'd check if any users are assigned this role before deleting,
            // or we'd nullify the roles of users. The prompt doesn't ask for user role nullifying for this step, but we will wrap it safely.
            try
            {
                _context.Roles.Remove(role);
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
