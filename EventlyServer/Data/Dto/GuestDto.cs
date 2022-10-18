namespace EventlyServer.Data.Dto;

public record GuestDto(int Id, string Name);

public record GuestCreatingDto(string Name, string PhoneNumber);