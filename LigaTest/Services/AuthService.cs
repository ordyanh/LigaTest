using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LigaTest.DTOs;
using LigaTest.Interfaces;
using LigaTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;
using Claim = System.Security.Claims.Claim;

namespace LigaTest.Services;

public class AuthService : IAuthService
{
    private readonly LigaContext _context;
    private readonly IConfiguration _config;

    public AuthService(LigaContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email)) return null;

        var user = new User
        {
            Email = dto.Email,
            FullName = dto.FullName,
            Role = dto.Role, 
            PasswordHash = BC.HashPassword(dto.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new AuthResponseDto(GenerateToken(user), user.Email, user.Role, user.Id,user.FullName);
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null || !BC.Verify(dto.Password, user.PasswordHash)) return null;

        return new AuthResponseDto(GenerateToken(user), user.Email, user.Role, user.Id,user.FullName);
    }

    public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string role)
    {
        return await _context.Users
            .Where(u => u.Role == role)
            .Select(u => new UserDto(u.Id, u.Email, u.FullName))
            .ToListAsync();
    }

    private string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}