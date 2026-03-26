using LigaTest.DTOs;
using LigaTest.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto) => Ok(await _authService.RegisterAsync(dto));

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto) => Ok(await _authService.LoginAsync(dto));
}