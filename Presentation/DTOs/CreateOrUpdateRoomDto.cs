namespace Presentation.DTOs;

public class CreateOrUpdateRoomDto
{
    public string Number { get; set; }
    public int UseTypeId { get; set; }
    public int RoomTypeId { get; set; }
    public int HotelId { get; set; }
}
