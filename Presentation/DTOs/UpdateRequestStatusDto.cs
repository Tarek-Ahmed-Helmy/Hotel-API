namespace Presentation.DTOs;

public class UpdateRequestStatusDto
{
    public int RequestId { get; set; }
    public int NewStatusId { get; set; }
    public string? Response { get; set; }
    public List<ServiceStatusUpdateDto> Updates { get; set; }
}
