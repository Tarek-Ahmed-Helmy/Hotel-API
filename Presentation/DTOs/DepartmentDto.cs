using Domain.Models;

namespace Presentation.DTOs;

public class DepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<ApplicationUser>? Users { get; set; }
}
