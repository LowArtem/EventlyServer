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

/// <summary>
/// Гость, принявший приглашение на мероприятие
/// </summary>
public record GuestFullCreatingDto
{
    /// <summary>
    /// Гость, принявший приглашение на мероприятие
    /// </summary>
    /// <param name="name">Имя</param>
    /// <param name="phoneNumber">Номер телефона</param>
    /// <param name="idInvitation">ID принимаемого приглашения</param>
    public GuestFullCreatingDto(string name, string phoneNumber, int idInvitation)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        IdInvitation = idInvitation;
    }

    /// <summary>Имя</summary>
    [Required]
    public string Name { get; init; }

    /// <summary>Номер телефона</summary>
    [Required]
    public string PhoneNumber { get; init; }

    /// <summary>ID принимаемого приглашения</summary>
    [Required]
    public int IdInvitation { get; init; }
}