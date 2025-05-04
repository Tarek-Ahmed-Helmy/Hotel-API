namespace Domain.Models;

public class Status
{
    public int Id { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<RequestHeader>? RequestHeaders { get; set; }
    public ICollection<RequestDetails>? RequestDetails { get; set; }
}
