using Domain.Models;

namespace Presentation.DTOs;

public class UseTypeDto
{
    public int Id { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<RoomDto>? Rooms { get; set; }
}
