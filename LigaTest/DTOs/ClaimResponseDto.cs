namespace LigaTest.DTOs;

public class ClaimResponseDto
{
    public int Id { get; set; }
    public string PolicyNumber { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public DateTime IncidentDate { get; set; }
    public string Description { get; set; } = null!;
    public decimal RequestedAmount { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}