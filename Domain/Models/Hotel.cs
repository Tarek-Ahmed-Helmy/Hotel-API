namespace Domain.Models;

public class Hotel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CompanyId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public Company? Company { get; set; }
    public ICollection<ApplicationUser>? Users { get; set; }
    public ICollection<Room>? Rooms { get; set; }
}
