using System.ComponentModel.DataAnnotations;

namespace ComplyFlow.API.DTOs
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class CreateRoleDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateRoleDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
