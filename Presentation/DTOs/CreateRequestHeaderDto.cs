using Domain.Models;

namespace Presentation.DTOs;

public class CreateRequestHeaderDto
{
    public string CustName { get; set; }
    public string CustEmail { get; set; }
    public string CustPhone { get; set; }
    public int HotelId { get; set; }
    public int RoomId { get; set; }
    public string? Note { get; set; }
    public string? Reply { get; set; }
    public string? Review { get; set; }
    public ICollection<CreateRequestDetailsDto>? RequestDetails { get; set; }
}
