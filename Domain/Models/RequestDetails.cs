namespace Domain.Models;

public class RequestDetails
{
    public int Id { get; set; }
    public int StatusId { get; set; }
    public int RequestHeaderId { get; set; }
    public int ServiceId { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public Status? Status { get; set; }
    public RequestHeader? RequestHeader { get; set; }
    public Service? Service { get; set; }
}
