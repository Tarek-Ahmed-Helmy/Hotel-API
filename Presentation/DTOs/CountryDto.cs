namespace Presentation.DTOs;

public class CountryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<CompanyDto>? Companies { get; set; }
}
