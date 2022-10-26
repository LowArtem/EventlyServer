using EventlyServer.Data.Entities.Enums;

namespace EventlyServer.Data.Dto;

public record LandingInvitationDto(int Id, string Name, DateOnly StartDate, DateOnly FinishDate, TemplateDto Template,
    List<GuestDto> Guests, OrderStatuses OrderStatus = OrderStatuses.ACCEPTED, string? Link = null);

// TODO: обсудить со стороны фронтенда
public record LandingInvitationShortDto(int Id, string Name, DateOnly StartDate, DateOnly FinishDate, OrderStatuses OrderStatus);
    
public record LandingInvitationCreatingDto(string Name, DateOnly StartDate, DateOnly FinishDate, int IdTemplate,
    OrderStatuses OrderStatus = OrderStatuses.ACCEPTED, string? Link = null);
    
public record LandingInvitationUpdatingDto(int Id, string? Name = null, DateOnly? StartDate = null, DateOnly? FinishDate = null, 
    int? IdTemplate = null, int? IdClient = null, OrderStatuses? OrderStatus = null, string? Link = "");