using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;
using EventlyServer.Data.Entities.Enums;

namespace EventlyServer.Data.Mappers;

public static class LandingInvitationMapper
{
    public static LandingInvitationDto ToDto(this LandingInvitation landingInvitation) =>
        new LandingInvitationDto(
            landingInvitation.Id,
            landingInvitation.Name, landingInvitation.StartDate.ToDateTime(new TimeOnly(0, 0)),
            landingInvitation.FinishDate.ToDateTime(new TimeOnly(0, 0)),
            landingInvitation.ChosenTemplate.ToDto(), landingInvitation.Guests.ConvertAll(r => r.ToDto()),
            landingInvitation.OrderStatus, landingInvitation.Link
        );

    public static LandingInvitationShortDto ToShortDto(this LandingInvitation landingInvitation) =>
        new LandingInvitationShortDto(
            landingInvitation.Id, landingInvitation.Name, landingInvitation.StartDate.ToDateTime(new TimeOnly(0, 0)),
            landingInvitation.FinishDate.ToDateTime(new TimeOnly(0, 0)),
            landingInvitation.OrderStatus
        );

    public static LandingInvitation ToLandingInvitation(this LandingInvitationCreatingDto dto, int idClient) =>
        new LandingInvitation(
            dto.Name,
            DateOnly.FromDateTime(dto.StartDate),
            DateOnly.FromDateTime(dto.FinishDate), dto.IdTemplate,
            idClient
        );
}