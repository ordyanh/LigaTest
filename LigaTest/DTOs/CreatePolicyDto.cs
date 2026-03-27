namespace LigaTest.DTOs;

public record CreatePolicyDto(
    string PolicyNumber,
    int UserId,
    DateTime StartDate,
    DateTime EndDate,
    decimal MaxCompensation,
    decimal PremiumAmount
);