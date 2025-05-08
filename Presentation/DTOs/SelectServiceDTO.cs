namespace Presentation.DTOs;

public class SelectServiceDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<SelectServiceDTO> SubServices { get; set; } = new();
}
