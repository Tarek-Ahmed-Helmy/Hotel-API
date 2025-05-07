namespace Domain.Models;

public class RequestHeader
{
    public int Id { get; set; }
    public string CustName { get; set; }
    public string CustEmail { get; set; }
    public string CustPhone { get; set; }
    public string Code { get; set; }
    public string? Note { get; set; }
    public string? AttachmentPath { get; set; }
    public string? Reply { get; set; }
    public string? Review { get; set; }
    public int HotelId { get; set; }
    public int RoomId { get; set; }
    public int StatusId { get; set; }
    public Hotel? Hotel { get; set; }
    public Room? Room { get; set; }
    public Status? Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<RequestDetails>? RequestDetails { get; set; }
}
