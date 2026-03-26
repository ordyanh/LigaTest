using LigaTest.DTOs;
using LigaTest.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LigaTest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClaimsController : ControllerBase
{
    private readonly IClaimsService _claimsService;

    public ClaimsController(IClaimsService claimsService)
    {
        _claimsService = claimsService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateClaimDto dto)
    {
        try
        {
            var result = await _claimsService.CreateClaimAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? userId, [FromQuery] string? status)
    {
        // Сотрудник вызовет без userId, пользователь — со своим Id
        var claims = await _claimsService.GetClaimsAsync(userId, status);
        return Ok(claims);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus)
    {
        try
        {
            var success = await _claimsService.UpdateStatusAsync(id, newStatus);
            if (!success) return NotFound();
            return Ok("Статус обновлен");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}