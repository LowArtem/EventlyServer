using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;

namespace EventlyServer.Data.Mappers;

public static class ResponseMapper
{
    public static ResponseDto ToDto(this Response response) => new ResponseDto(response.Date, response.GuestNavigation.ToDto());

    public static Response ToResponse(this ResponseCreatingDto dto) =>
        new Response(dto.Date, dto.LandingInvitationId, dto.GuestId);
    
    public static Response ToResponse(this ResponseFullCreatingDto dto) =>
        new Response(dto.Date, dto.IdLandingInvitation, dto.Guest.ToGuest());
}