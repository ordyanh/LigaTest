using System.Security.Claims;
using LigaTest.DTOs;
using LigaTest.Interfaces;
using LigaTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LigaTest.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PoliciesController : ControllerBase
{
    private readonly IPolicyService _policyService;
    private readonly LigaContext _context;

    public PoliciesController(IPolicyService policyService, LigaContext context)
    {
        _policyService = policyService;
        _context = context;
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyPolicies()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized();

        int userId = int.Parse(userIdClaim);
        var policies = await _policyService.GetUserPoliciesAsync(userId);

        return Ok(policies);
    }


    [HttpGet("all")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> GetAllPolicies()
    {
        var policies = await _policyService.GetAllPoliciesAsync();
        return Ok(policies);
    }

 
    [HttpPost]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> CreatePolicy([FromBody] CreatePolicyDto dto)
    {
        try
        {
            var result = await _policyService.CreatePolicyAsync(dto);
            return Ok(new { Message = "Policy created successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

  
    [HttpGet("users")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> GetClientsList()
    {
        var users = await _context.Users
            .Where(u => u.Role == "User")
            .Select(u => new {
                u.Id,
                u.FullName,
                u.Email
            })
            .ToListAsync();

        return Ok(users);
    }
}