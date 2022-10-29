using EventlyServer.Data.Entities.Abstract;
using EventlyServer.Data.Entities.Enums;

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
    public virtual List<Guest> Guests { get; set; }

    public LandingInvitation()
    {
        Guests = new List<Guest>();
    }
    
    public LandingInvitation(string name, DateOnly startDate, DateOnly finishDate, int idTemplate, int idClient,
        OrderStatuses orderStatus = OrderStatuses.ACCEPTED, string? link = null)
    {
        Name = name;
        StartDate = startDate;
        FinishDate = finishDate;
        IdTemplate = idTemplate;
        IdClient = idClient;
        OrderStatus = orderStatus;
        Link = link;
        
        Guests = new List<Guest>();
    }

    public LandingInvitation(int id, string? link, string name, OrderStatuses orderStatus, DateOnly startDate, 
        DateOnly finishDate, int idClient, int idTemplate)
    {
        Id = id;
        Link = link;
        Name = name;
        OrderStatus = orderStatus;
        StartDate = startDate;
        FinishDate = finishDate;
        IdClient = idClient;
        IdTemplate = idTemplate;
        
        Guests = new List<Guest>();
    }
}