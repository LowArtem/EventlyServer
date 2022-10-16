namespace EventlyServer.Data.Dto;

public record UserDto(string Name, string Email, string Password, string? PhoneNumber,
    string? OtherCommunication = null, bool IsAdmin = false);