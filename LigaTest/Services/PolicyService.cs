using LigaTest.DTOs;
using LigaTest.Interfaces;
using LigaTest.Models;
using Microsoft.EntityFrameworkCore;

namespace LigaTest.Services;

public class PolicyService : IPolicyService
{
    private readonly LigaContext _context;

    public PolicyService(LigaContext context)
    {
        _context = context;
    }

    public async Task<bool> CreatePolicyAsync(CreatePolicyDto dto)
    {
        var exists = await _context.Policies.AnyAsync(p => p.PolicyNumber == dto.PolicyNumber);
        if (exists)
        {
            throw new Exception($"Policy with number {dto.PolicyNumber} already exists.");
        }

        if (dto.StartDate >= dto.EndDate)
        {
            throw new Exception("Start date must be earlier than end date.");
        }

        var policy = new Policy
        {
            PolicyNumber = dto.PolicyNumber,
            UserId = dto.UserId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            MaxCompensation = dto.MaxCompensation,
            PremiumAmount = dto.PremiumAmount
        };

        _context.Policies.Add(policy);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<PolicyResponseDto>> GetUserPoliciesAsync(int userId)
    {
        return await _context.Policies
            .Where(p => p.UserId == userId)
            .Select(p => new PolicyResponseDto
            {
                Id = p.Id,
                PolicyNumber = p.PolicyNumber,
                ClientName = p.User.FullName,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                MaxCompensation = p.MaxCompensation,
                PremiumAmount = p.PremiumAmount
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<PolicyResponseDto>> GetAllPoliciesAsync()
    {
        return await _context.Policies
            .Include(p => p.User)
            .Select(p => new PolicyResponseDto
            {
                Id = p.Id,
                PolicyNumber = p.PolicyNumber,
                ClientName = p.User.FullName,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                MaxCompensation = p.MaxCompensation,
                PremiumAmount = p.PremiumAmount
            })
            .ToListAsync();
    }
}