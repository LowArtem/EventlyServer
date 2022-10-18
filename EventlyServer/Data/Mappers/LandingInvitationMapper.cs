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
    dto.Name, dto.StartDate, dto.FinishDate, dto.Template, dto.OrderStatus, dto.Link);
}