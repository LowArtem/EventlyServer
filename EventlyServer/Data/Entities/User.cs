using EventlyServer.Data.Entities.Abstract;

namespace EventlyServer.Data.Entities;

public partial class User : Entity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? OtherCommunication { get; set; }
    public bool IsAdmin { get; set; }

    public virtual List<LandingInvitation> LandingInvitations { get; set; }

    public User()
    {
        LandingInvitations = new List<LandingInvitation>();
    }

    public User(string name, string email, string password, string? phoneNumber, string? otherCommunication = null, bool isAdmin = false)
    {
        Name = name;
        Email = email;
        Password = password;
        PhoneNumber = phoneNumber;
        OtherCommunication = otherCommunication;
        IsAdmin = isAdmin;
        
        LandingInvitations = new List<LandingInvitation>();
    }
}