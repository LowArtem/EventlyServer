using System.ComponentModel.DataAnnotations;
using EventlyServer.Data.Entities.Enums;

namespace EventlyServer.Data.Dto;

/// <summary>
/// Заказ приглашения
/// </summary>
public record LandingInvitationDto
{
    /// <summary>
    /// Заказ приглашения
    /// </summary>
    /// <param name="id">ID заказа</param>
    /// <param name="name">Название приглашения (видимое и создаваемое пользователем)</param>
    /// <param name="startDate">Дата начала работы готового лендинга-приглашения</param>
    /// <param name="finishDate">Дата окончания работы готового лендинга-приглашения</param>
    /// <param name="template">Выбранный шаблон приглашения</param>
    /// <param name="guests">Список гостей, откликнувшихся на это приглашение</param>
    /// <param name="orderStatus">Текущий статус заказа</param>
    /// <param name="link">Ссылка на сайт-приглашение (если он уже готов)</param>
    public LandingInvitationDto(int id, string name, DateTime startDate, DateTime finishDate, TemplateDto template,
        List<GuestDto> guests, OrderStatuses orderStatus = OrderStatuses.ACCEPTED, string? link = null)
    {
        Id = id;
        Name = name;
        StartDate = startDate;
        FinishDate = finishDate;
        Template = template;
        Guests = guests;
        OrderStatus = orderStatus;
        Link = link;
    }

    /// <summary>ID заказа</summary>
    [Required]
    public int Id { get; init; }

    /// <summary>Название приглашения (видимое и создаваемое пользователем)</summary>
    [Required]
    public string Name { get; init; }

    /// <summary>Дата начала работы готового лендинга-приглашения</summary>
    [Required]
    public DateTime StartDate { get; init; }

    /// <summary>Дата окончания работы готового лендинга-приглашения</summary>
    [Required]
    public DateTime FinishDate { get; init; }

    /// <summary>Выбранный шаблон приглашения</summary>
    [Required]
    public TemplateDto Template { get; init; }

    /// <summary>Список гостей, откликнувшихся на это приглашение</summary>
    public List<GuestDto> Guests { get; init; }

    /// <summary>Текущий статус заказа</summary>
    [Required]
    public OrderStatuses OrderStatus { get; init; }

    /// <summary>Ссылка на сайт-приглашение (если он уже готов)</summary>
    public string? Link { get; init; }
}

// TODO: обсудить со стороны фронтенда
/// <summary>
/// Заказ приглашения
/// </summary>
public record LandingInvitationShortDto
{
    /// <summary>
    /// Заказ приглашения
    /// </summary>
    /// <param name="id">ID заказа</param>
    /// <param name="name">Название приглашения (видимое и создаваемое пользователем)</param>
    /// <param name="startDate">Дата начала работы готового лендинга-приглашения</param>
    /// <param name="finishDate">Дата окончания работы готового лендинга-приглашения</param>
    /// <param name="orderStatus">Текущий статус заказа</param>
    public LandingInvitationShortDto(int id, string name, DateTime startDate, DateTime finishDate, OrderStatuses orderStatus)
    {
        Id = id;
        Name = name;
        StartDate = startDate;
        FinishDate = finishDate;
        OrderStatus = orderStatus;
    }

    /// <summary>ID заказа</summary>
    [Required]
    public int Id { get; init; }

    /// <summary>Название приглашения (видимое и создаваемое пользователем)</summary>
    [Required]
    public string Name { get; init; }

    /// <summary>Дата начала работы готового лендинга-приглашения</summary>
    [Required]
    public DateTime StartDate { get; init; }

    /// <summary>Дата окончания работы готового лендинга-приглашения</summary>
    [Required]
    public DateTime FinishDate { get; init; }

    /// <summary>Текущий статус заказа</summary>
    [Required]
    public OrderStatuses OrderStatus { get; init; }
}

/// <summary>
/// Заказ приглашения
/// </summary>
public record LandingInvitationCreatingDto
{
    /// <summary>
    /// Заказ приглашения
    /// </summary>
    /// <param name="name">Название приглашения (видимое и создаваемое пользователем)</param>
    /// <param name="startDate">Дата начала работы готового лендинга-приглашения</param>
    /// <param name="finishDate">Дата окончания работы готового лендинга-приглашения</param>
    /// <param name="idTemplate">ID выбранного шаблона</param>
    /// <param name="idClient">ID клиента, заказывающего приглашение (текущий пользователь)</param>
    public LandingInvitationCreatingDto(string name, DateTime startDate, DateTime finishDate, int idTemplate, int idClient)
    {
        Name = name;
        StartDate = startDate;
        FinishDate = finishDate;
        IdTemplate = idTemplate;
        IdClient = idClient;
    }

    /// <summary>Название приглашения (видимое и создаваемое пользователем)</summary>
    [Required]
    public string Name { get; init; }

    /// <summary>Дата начала работы готового лендинга-приглашения</summary>
    [Required]
    public DateTime StartDate { get; init; }

    /// <summary>Дата окончания работы готового лендинга-приглашения</summary>
    [Required]
    public DateTime FinishDate { get; init; }

    /// <summary>ID выбранного шаблона</summary>
    [Required]
    public int IdTemplate { get; init; }

    /// <summary>ID клиента, заказывающего приглашение (текущий пользователь)</summary>
    [Required]
    public int IdClient { get; init; }
}

/// <summary>
/// Заказ приглашения (информация для изменения)
/// </summary>
/// <remarks>
/// Все поля, кроме ID, необязательны для заполнения - их нужно заполнять, если нужно обновить значение.
///<para></para>
/// Если нужно оставить значение без изменения - передать null (исключение: поле Link - оставить
/// без изменений - передать пустую строку)
/// </remarks>
public record LandingInvitationUpdatingDto
{
    /// <summary>
    /// Заказ приглашения (информация для изменения)
    /// </summary>
    /// <param name="id">ID заказа</param>
    /// <param name="name">Название приглашения (видимое и создаваемое пользователем)</param>
    /// <param name="startDate">Дата начала работы готового лендинга-приглашения</param>
    /// <param name="finishDate">Дата окончания работы готового лендинга-приглашения</param>
    /// <param name="idTemplate">ID выбранного шаблона приглашения</param>
    /// <param name="orderStatus">Текущий статус заказа</param>
    /// <param name="link">Ссылка на сайт-приглашение (если он уже готов)</param>
    public LandingInvitationUpdatingDto(int id, string? name = null, DateTime? startDate = null, DateTime? finishDate = null, 
        int? idTemplate = null, OrderStatuses? orderStatus = null, string? link = "")
    {
        Id = id;
        Name = name;
        StartDate = startDate;
        FinishDate = finishDate;
        IdTemplate = idTemplate;
        OrderStatus = orderStatus;
        Link = link;
    }

    /// <summary>ID заказа</summary>
    [Required]
    public int Id { get; init; }

    /// <summary>Название приглашения (видимое и создаваемое пользователем)</summary>
    public string? Name { get; init; }

    /// <summary>Дата начала работы готового лендинга-приглашения</summary>
    public DateTime? StartDate { get; init; }

    /// <summary>Дата окончания работы готового лендинга-приглашения</summary>
    public DateTime? FinishDate { get; init; }

    /// <summary>ID выбранного шаблона приглашения</summary>
    public int? IdTemplate { get; init; }

    /// <summary>Текущий статус заказа</summary>
    public OrderStatuses? OrderStatus { get; init; }

    /// <summary>Ссылка на сайт-приглашение (если он уже готов)</summary>
    public string? Link { get; init; }
}