using LigaTest.DTOs;

namespace LigaTest.Interfaces;

public interface IClaimsService
{
    Task<ClaimResponseDto> CreateClaimAsync(CreateClaimDto dto);
    Task<IEnumerable<ClaimResponseDto>> GetClaimsAsync(int? userId, string? status);
    Task<bool> UpdateStatusAsync(int claimId, string newStatus);
}