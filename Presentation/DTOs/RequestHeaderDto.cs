namespace Presentation.DTOs;

public class RequestHeaderDto
{
    public int Id { get; set; }
    public string CustName { get; set; }
    public string CustEmail { get; set; }
    public string CustPhone { get; set; }
    public string Code { get; set; }
    public string? Note { get; set; }
    public string? SpecialRequest { get; set; }
    public string? AttachmentPath { get; set; }
    public string? Reply { get; set; }
    public string? Review { get; set; }
    public string HotelName { get; set; }
    public string RoomNumber { get; set; }
    public string StatusName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<RequestDetailsDto>? RequestDetails { get; set; }
}
