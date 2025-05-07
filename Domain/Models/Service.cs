namespace Domain.Models;

public class Service
{
    public int Id { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public ServiceLevel ServiceLevel { get; set; }
    public int? SuperServiceId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public Service? SuperService { get; set; }
    public ICollection<RequestDetails>? RequestDetails { get; set; }
}


public enum ServiceLevel
{
    MainService = 1,
    SubService = 2,
    ServiceComponent = 3
}