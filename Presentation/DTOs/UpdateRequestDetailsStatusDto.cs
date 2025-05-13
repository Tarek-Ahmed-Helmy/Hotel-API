namespace Presentation.DTOs;

public class UpdateRequestDetailsStatusDto
{
    public int RequestDetailsId { get; set; }
    public string? Status { get; set; }
    public string? Note { get; set; }
    public string? UserId { get; set; }

}
