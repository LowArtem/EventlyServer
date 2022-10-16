
namespace EventlyServer.Data.Entities;

public partial class Response
{
    public DateOnly Date { get; set; }
    public int IdGuest { get; set; }
    public int IdLandingInvitation { get; set; }

    public virtual Guest GuestNavigation { get; set; } = null!;
    public virtual LandingInvitation LandingInvitationNavigation { get; set; } = null!;
}