namespace Domain.Models;

public class RoomType
{
    public int Id { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Room>? Rooms { get; set; }
}
