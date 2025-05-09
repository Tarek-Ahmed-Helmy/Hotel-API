namespace Presentation.DTOs;

public class UserRequestHeaderDto
{
    public string CustName { get; set; }
    public string CustEmail { get; set; }
    public string CustPhone { get; set; }
    public string? Reply { get; set; }
    public string? SpecialRequest { get; set; }
    public string HotelName { get; set; }
    public string RoomNumber { get; set; }
    public string StatusName { get; set; }
    public ICollection<RequestDetailsDto>? RequestDetails { get; set; }
}
