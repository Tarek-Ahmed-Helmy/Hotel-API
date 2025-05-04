namespace Presentation.DTOs;

public class CompanyDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string CountryName { get; set; }
    public ICollection<HotelDto>? Hotels { get; set; }
}
