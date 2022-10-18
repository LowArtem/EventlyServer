namespace EventlyServer.Data.Dto;

public record ResponseDto(DateTime Date, GuestDto Guest);

public record ResponseFullCreatingDto(DateTime Date, GuestCreatingDto Guest, int IdLandingInvitation);

public record ResponseCreatingDto(DateTime Date, int GuestId, int LandingInvitationId);