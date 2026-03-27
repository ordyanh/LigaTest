namespace LigaTest.DTOs;

public record RegisterDto(string Email, string Password, string FullName, string Role);
public record LoginDto(string Email, string Password);
public record AuthResponseDto(string Token, string Email, string Role, int UserId, string FullName);
public record UserDto(int Id, string Email, string FullName);

