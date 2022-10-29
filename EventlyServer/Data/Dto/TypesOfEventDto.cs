namespace EventlyServer.Data.Dto;

/// <summary>
/// Тип мероприятия
/// </summary>
public record TypesOfEventDto
{
    /// <summary>
    /// Тип мероприятия
    /// </summary>
    /// <param name="id">ID типа</param>
    /// <param name="name">Название</param>
    public TypesOfEventDto(int id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <summary>ID типа</summary>
    public int Id { get; init; }

    /// <summary>Название</summary>
    public string Name { get; init; }
}