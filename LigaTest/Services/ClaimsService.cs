using LigaTest.DTOs;
using LigaTest.Interfaces;
using LigaTest.Models;
using Microsoft.EntityFrameworkCore;

namespace LigaTest.Services;

public class ClaimsService : IClaimsService
{
    private readonly LigaContext _context;

    public ClaimsService(LigaContext context)
    {
        _context = context;
    }

    // ЛОГИКА 1: Создание заявки
    public async Task<ClaimResponseDto> CreateClaimAsync(CreateClaimDto dto)
    {
        var policy = await _context.Policies.FindAsync(dto.PolicyId);
        if (policy == null || policy.UserId != dto.UserId)
            throw new Exception("Полис не найден или не принадлежит пользователю");

        var claim = new Claim
        {
            PolicyId = dto.PolicyId,
            UserId = dto.UserId,
            IncidentDate = dto.IncidentDate,
            Description = dto.Description,
            RequestedAmount = dto.RequestedAmount,
            Status = "Created", // Авто-статус
            CreatedAt = DateTime.Now
        };

        _context.Claims.Add(claim);
        await _context.SaveChangesAsync();

        return MapToDto(claim, policy.PolicyNumber, ""); // Имя можно подтянуть позже
    }

    // ЛОГИКА 2: Получение списка с фильтрацией
    public async Task<IEnumerable<ClaimResponseDto>> GetClaimsAsync(int? userId, string? status)
    {
        var query = _context.Claims
            .Include(c => c.Policy)
            .Include(c => c.User)
            .AsQueryable();

        // Если передан userId — фильтруем (для обычного пользователя)
        if (userId.HasValue)
            query = query.Where(c => c.UserId == userId.Value);

        // Фильтрация по статусу
        if (!string.IsNullOrEmpty(status))
            query = query.Where(c => c.Status == status);

        var claims = await query.ToListAsync();
        return claims.Select(c => MapToDto(c, c.Policy.PolicyNumber, c.User.FullName));
    }

    // ЛОГИКА 3: Изменение статуса (только для сотрудника)
    public async Task<bool> UpdateStatusAsync(int claimId, string newStatus)
    {
        var claim = await _context.Claims.FindAsync(claimId);
        if (claim == null) return false;

        // Бизнес-правило: менять можно только из статуса "Created"
        if (claim.Status != "Created")
            throw new Exception("Нельзя изменить статус уже обработанной заявки");

        claim.Status = newStatus;
        await _context.SaveChangesAsync();
        return true;
    }

    // Маппинг (в идеале использовать AutoMapper)
    private static ClaimResponseDto MapToDto(Claim c, string policyNum, string userName) => new()
    {
        Id = c.Id,
        PolicyNumber = policyNum,
        UserName = userName,
        IncidentDate = c.IncidentDate,
        Description = c.Description,
        RequestedAmount = c.RequestedAmount,
        Status = c.Status,
        CreatedAt = c.CreatedAt
    };
}