using System.Security.Claims;
using LigaTest.DTOs;
using LigaTest.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LigaTest.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClaimsController : ControllerBase
{
    private readonly IClaimsService _claimsService;

    public ClaimsController(IClaimsService claimsService) => _claimsService = claimsService;

    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> Create(CreateClaimDto dto)
    {
        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            dto.UserId = userId;

            var result = await _claimsService.CreateClaimAsync(dto);
            return Ok(result);
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] string? status)
    {
        var role = User.FindFirstValue(ClaimTypes.Role);
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var filterUserId = role == "Employee" ? null : (int?)userId;

        var claims = await _claimsService.GetClaimsAsync(filterUserId, status);
        return Ok(claims);
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] string newStatus)
    {
        try
        {
            var result = await _claimsService.UpdateStatusAsync(id, newStatus);
            return Ok(new { Message = "Статус обновлен" });
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }
}