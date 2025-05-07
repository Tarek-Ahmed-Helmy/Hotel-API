namespace Presentation.DTOs;

public class RequestDetailsDto
{
    public int Id { get; set; }
    public string StatusName { get; set; }
    public string ServiceName { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}