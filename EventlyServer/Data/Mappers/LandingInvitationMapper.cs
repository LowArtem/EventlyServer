using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities;

namespace EventlyServer.Data.Mappers;

public static class LandingInvitationMapper
{
    public static LandingInvitationDto ToDto(this LandingInvitation landingInvitation) => new LandingInvitationDto(
        landingInvitation.Id,
        landingInvitation.Name, landingInvitation.StartDate, landingInvitation.FinishDate,
        landingInvitation.ChosenTemplate.ToDto(), landingInvitation.Responses.ConvertAll(r => r.ToDto()),
        landingInvitation.OrderStatus, landingInvitation.Link);

    public static LandingInvitation ToLandingInvitation(this LandingInvitationCreatingDto dto) => new LandingInvitation(
        dto.Name, dto.StartDate, dto.FinishDate, dto.IdTemplate, dto.OrderStatus, dto.Link);

    public static LandingInvitation ToLandingInvitation(this LandingInvitationUpdatingDto dto) => new LandingInvitation(
        dto.Id, dto.Link, dto.Name, dto.OrderStatus, dto.StartDate, dto.FinishDate, dto.IdClient, dto.IdTemplate
    );
}