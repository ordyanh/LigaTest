using LigaTest.DTOs;
using LigaTest.Models;

namespace LigaTest.Interfaces;

public interface IPolicyService
{
    Task<bool> CreatePolicyAsync(CreatePolicyDto dto);
    Task<IEnumerable<PolicyResponseDto>> GetUserPoliciesAsync(int userId);
    Task<IEnumerable<PolicyResponseDto>> GetAllPoliciesAsync();
}