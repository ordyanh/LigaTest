using LigaTest.DTOs;
using LigaTest.Interfaces;
using LigaTest.Models;
using Microsoft.EntityFrameworkCore;

namespace LigaTest.Services;

public class ClaimsService : IClaimsService
{
    private readonly LigaContext _context;
    public ClaimsService(LigaContext context) => _context = context;

    public async Task<ClaimResponseDto> CreateClaimAsync(CreateClaimDto dto)
    {
        var policy = await _context.Policies.FindAsync(dto.PolicyId);

        // ВАЛИДАЦИЯ
        if (policy == null || policy.UserId != dto.UserId)
            throw new Exception("Полис не найден или не принадлежит вам.");

        if (dto.IncidentDate < policy.StartDate || dto.IncidentDate > policy.EndDate)
            throw new Exception("Дата инцидента вне срока действия полиса.");

        if (dto.RequestedAmount > policy.MaxCompensation)
            throw new Exception("Сумма превышает максимальный лимит полиса.");

        var claim = new Claim
        {
            PolicyId = dto.PolicyId,
            UserId = dto.UserId,
            IncidentDate = dto.IncidentDate,
            Description = dto.Description,
            RequestedAmount = dto.RequestedAmount,
            Status = "Created",
            CreatedAt = DateTime.Now
        };

        _context.Claims.Add(claim);
        await _context.SaveChangesAsync();

        return new ClaimResponseDto { Id = claim.Id, Status = claim.Status }; // Упрощено
    }

    public async Task<IEnumerable<ClaimResponseDto>> GetClaimsAsync(int? userId, string? status)
    {
        var query = _context.Claims.Include(c => c.Policy).Include(c => c.User).AsQueryable();

        if (userId.HasValue) query = query.Where(c => c.UserId == userId);
        if (!string.IsNullOrEmpty(status)) query = query.Where(c => c.Status == status);

        return await query.Select(c => new ClaimResponseDto
        {
            Id = c.Id,
            PolicyNumber = c.Policy.PolicyNumber,
            UserName = c.User.FullName,
            Status = c.Status,
            CreatedAt = c.CreatedAt,
            Description = c.Description,
            RequestedAmount = c.RequestedAmount
        }).ToListAsync();
    }

    public async Task<bool> UpdateStatusAsync(int claimId, string newStatus)
    {
        var claim = await _context.Claims.FindAsync(claimId);
        if (claim == null) return false;
        if (claim.Status != "Created") throw new Exception("Можно менять статус только новых заявок.");

        claim.Status = newStatus;
        await _context.SaveChangesAsync();
        return true;
    }
}