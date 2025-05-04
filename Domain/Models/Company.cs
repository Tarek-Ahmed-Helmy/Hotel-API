namespace Domain.Models;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CountryId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public Country? Country { get; set; }
    public ICollection<Hotel>? Hotels { get; set; }
}
