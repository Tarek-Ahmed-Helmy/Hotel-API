using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Department
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public ICollection<ApplicationUser>? Users { get; set; }
}
