using EventlyServer.Data.Entities.Enums;

namespace EventlyServer.Data.Dto;

public record LandingInvitationDto(int Id, string Name, DateOnly StartDate, DateOnly FinishDate, TemplateDto Template,
    List<ResponseDto> GuestResponses, OrderStatuses OrderStatus = OrderStatuses.ACCEPTED, string? Link = null);
    
public record LandingInvitationCreatingDto(string Name, DateOnly StartDate, DateOnly FinishDate, int IdTemplate,
    OrderStatuses OrderStatus = OrderStatuses.ACCEPTED, string? Link = null);
    
public record LandingInvitationUpdatingDto(int Id, string Name, DateOnly StartDate, DateOnly FinishDate, 
    int IdTemplate, int IdClient, OrderStatuses OrderStatus = OrderStatuses.ACCEPTED, string? Link = null);