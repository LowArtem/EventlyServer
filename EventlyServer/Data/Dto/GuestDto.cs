namespace EventlyServer.Data.Dto;

public record GuestDto(int Id, string Name);

public record GuestFullCreatingDto(string Name, string PhoneNumber, int IdInvitation);