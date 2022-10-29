using System.ComponentModel.DataAnnotations;

namespace EventlyServer.Data.Dto;

/// <summary>
/// Гость, принявший приглашение на мероприятие
/// </summary>
public record GuestDto
{
    /// <summary>
    /// Гость, принявший приглашение на мероприятие
    /// </summary>
    /// <param name="id">ID гостя</param>
    /// <param name="name">Имя</param>
    public GuestDto(int id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <summary>ID гостя</summary>
    [Required]
    public int Id { get; init; }

    /// <summary>Имя</summary>
    [Required]
    public string Name { get; init; }
}

public record GuestFullCreatingDto(string Name, string PhoneNumber, int IdInvitation);