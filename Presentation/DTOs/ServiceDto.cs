using Domain.Models;

namespace Presentation.DTOs;

public class ServiceDto
{
    public int Id { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public int? SuperServiceId { get; set; }
    public string? SuperServiceNameAr { get; set; }
    public string? SuperServiceNameEn { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<RequestDetails>? RequestDetails { get; set; }
}
