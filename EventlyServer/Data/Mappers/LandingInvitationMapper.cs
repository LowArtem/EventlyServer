using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;

namespace EventlyServer.Data.Mappers;

public static class LandingInvitationMapper
{
    public static LandingInvitationDto ToDto(this LandingInvitation landingInvitation) => new LandingInvitationDto(
        landingInvitation.Id,
        landingInvitation.Name, landingInvitation.StartDate, landingInvitation.FinishDate,
        landingInvitation.ChosenTemplate.ToDto(), landingInvitation.Guests.ConvertAll(r => r.ToDto()),
        landingInvitation.OrderStatus, landingInvitation.Link);

    public static LandingInvitationShortDto ToShortDto(this LandingInvitation landingInvitation) =>
        new LandingInvitationShortDto(
            landingInvitation.Id, landingInvitation.Name, landingInvitation.StartDate, landingInvitation.FinishDate,
            landingInvitation.OrderStatus
        );

    public static LandingInvitation ToLandingInvitation(this LandingInvitationCreatingDto dto) => new LandingInvitation(
        dto.Name, dto.StartDate, dto.FinishDate, dto.IdTemplate, dto.IdClient, dto.OrderStatus, dto.Link);
}