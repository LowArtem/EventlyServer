namespace EventlyServer.Data.Dto;

public record UserDto(int Id, string Name, string Email, string Password, string? PhoneNumber,
    string? OtherCommunication = null, bool IsAdmin = false);

public record UserUpdateDto(int Id, string? Name = null, string? Password = null, string? PhoneNumber = null,
    string? OtherCommunication = "");

public record UserAuthDto(string Email, string Password);