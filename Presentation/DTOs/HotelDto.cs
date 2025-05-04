using Domain.Models;

namespace Presentation.DTOs;

public class HotelDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CompanyId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string CompanyName { get; set; }
    public ICollection<ApplicationUser>? Users { get; set; }
    public ICollection<RoomDto>? Rooms { get; set; }
}
