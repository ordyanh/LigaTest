namespace LigaTest.DTOs;

public class PolicyResponseDto
{
    public int Id { get; set; }
    public string PolicyNumber { get; set; } = null!;
    public string ClientName { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal MaxCompensation { get; set; }
    public decimal PremiumAmount { get; set; }
}