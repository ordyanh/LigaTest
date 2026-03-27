using LigaTest.DTOs;

namespace LigaTest.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto?> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string role);

}