﻿
namespace EventlyServer.Data.Entities;

/// <summary>
/// Сущность отклика пользователя на приглашение
/// </summary>
public partial class Response
{
    public DateTime Date { get; set; }
    public int IdGuest { get; set; }
    public int IdLandingInvitation { get; set; }

    public virtual Guest GuestNavigation { get; set; } = null!;
    public virtual LandingInvitation LandingInvitationNavigation { get; set; } = null!;
}