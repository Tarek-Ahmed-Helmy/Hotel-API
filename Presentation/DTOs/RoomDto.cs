namespace Presentation.DTOs;

public class RoomDto
{
    public int Id { get; set; }
    public string Number { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UseTypeNameAr { get; set; }
    public string UseTypeNameEn { get; set; }
    public string RoomTypeNameAr { get; set; }
    public string RoomTypeNameEn { get; set; }
    public string HotelName { get; set; }
    public int HotelId { get; set; }
    public int RoomTypeId { get; set; }
    public int UseTypeId { get; set; }
}
