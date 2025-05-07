using Domain.Models;

namespace Presentation.DTOs;

public class CreateOrUpdateServiceDto
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public ServiceLevel ServiceLevel { get; set; }
    public int? SuperServiceId { get; set; }
}
