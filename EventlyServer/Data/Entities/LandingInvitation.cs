using EventlyServer.Data.Dto;
using EventlyServer.Data.Entities.Abstract;
using EventlyServer.Data.Entities.Enums;
using EventlyServer.Data.Mappers;

namespace EventlyServer.Data.Entities;

/// <summary>
/// Сущность приглашения. Содержит в себе информацию о заказе
/// </summary>
public partial class LandingInvitation : Entity
{
    public string? Link { get; set; } = null;
    public string Name { get; set; } = null!;
    public OrderStatuses OrderStatus { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly FinishDate { get; set; }
    public int IdClient { get; set; }
    public int IdTemplate { get; set; }

    public virtual User Client { get; set; } = null!;
    public virtual Template ChosenTemplate { get; set; } = null!;
    public virtual List<Response> Responses { get; set; }

    public LandingInvitation()
    {
        Responses = new List<Response>();
    }
    
    public LandingInvitation(string name, DateOnly startDate, DateOnly finishDate, TemplateDto template,
        OrderStatuses orderStatus = OrderStatuses.ACCEPTED, string? link = null)
    {
        Name = name;
        StartDate = startDate;
        FinishDate = finishDate;
        ChosenTemplate = template.ToTemplate();
        OrderStatus = orderStatus;
        Link = link;
        
        Responses = new List<Response>();
    }

}