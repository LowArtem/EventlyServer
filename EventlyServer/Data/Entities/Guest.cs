using EventlyServer.Data.Entities.Abstract;

namespace EventlyServer.Data.Entities;

/// <summary>
/// Сущности гостя сервиса (гость может принять участие в мероприятии)
/// </summary>
public partial class Guest : Entity
{
    public string Name { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

    public virtual List<Response> Responses { get; set; }

    public Guest()
    {
        Responses = new List<Response>();
    }
}