using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class ApplicationUser : IdentityUser
{
    [MaxLength(100)]
    public string FirstName { get; set; }
    
    [MaxLength(100)]
    public string LastName { get; set; }

    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
    public int HotelId { get; set; }
    public Hotel? Hotel { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<ApplicationRole>? Roles { get; set; }
}
