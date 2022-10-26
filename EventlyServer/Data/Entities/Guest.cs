using EventlyServer.Data.Entities.Abstract;

namespace EventlyServer.Data.Entities;

/// <summary>
/// Сущности гостя сервиса (гость может принять участие в мероприятии)
/// </summary>
public partial class Guest : Entity
{
    public string Name { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    
    public int IdLandingInvitation { get; set; }


    public virtual LandingInvitation Invitation { get; set; } = null!;

    public Guest()
    {
        
    }

    public Guest(string name, string phoneNumber)
    {
        Name = name;
        PhoneNumber = phoneNumber;
    }
    
    public Guest(string name, string phoneNumber, int invitationId)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        IdLandingInvitation = invitationId;
    }
    
}