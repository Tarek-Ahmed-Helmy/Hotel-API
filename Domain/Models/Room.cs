namespace Domain.Models;

public class Room
{
    public int Id { get; set; }
    public string Number { get; set; }
    public bool IsAvailable { get; set; } = true;
    public int UseTypeId { get; set; }
    public int RoomTypeId { get; set; }
    public int HotelId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public UseType? UseType { get; set; }
    public RoomType? RoomType { get; set; }
    public Hotel? Hotel { get; set; }
}
