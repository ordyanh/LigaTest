using System.Security.Claims;
using LigaTest.DTOs;
using LigaTest.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LigaTest.Controllers;

[Authorize] // Весь контроллер защищен
[ApiController]
[Route("api/[controller]")]
public class ClaimsController : ControllerBase
{
    private readonly IClaimsService _claimsService;

    public ClaimsController(IClaimsService claimsService) => _claimsService = claimsService;

    [HttpPost]
    [Authorize(Roles = "User")] // Только пользователь создает
    public async Task<IActionResult> Create(CreateClaimDto dto)
    {
        try
        {
            // Берем UserId из токена для безопасности
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

        // Если сотрудник - видит всех, если юзер - только своих
        var filterUserId = role == "Employee" ? null : (int?)userId;

        var claims = await _claimsService.GetClaimsAsync(filterUserId, status);
        return Ok(claims);
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Employee")] // Только сотрудник меняет статус
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