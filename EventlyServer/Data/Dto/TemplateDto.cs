using System.ComponentModel.DataAnnotations;

namespace EventlyServer.Data.Dto;

/// <summary>
/// Шаблон приглашения
/// </summary>
public record TemplateDto
{
    /// <summary>
    /// Шаблон приглашения
    /// </summary>
    /// <param name="id">ID шаблона</param>
    /// <param name="price">Цена</param>
    /// <param name="name">Название</param>
    /// <param name="event">Тип мероприятия</param>
    /// <param name="filePath">Путь к html-файлу шаблона</param>
    /// <param name="previewPath">Путь к png-изображению - превью шаблона</param>
    public TemplateDto(int id, int price, string name, TypesOfEventDto @event, string filePath, string previewPath)
    {
        Id = id;
        Price = price;
        Name = name;
        Event = @event;
        FilePath = filePath;
        PreviewPath = previewPath;
    }

    /// <summary>ID шаблона</summary>
    public int Id { get; init; }

    /// <summary>Цена</summary>
    public int Price { get; init; }

    /// <summary>Название</summary>
    public string Name { get; init; }

    /// <summary>Тип мероприятия</summary>
    public TypesOfEventDto Event { get; init; }

    /// <summary>Путь к html-файлу шаблона</summary>
    public string FilePath { get; init; }

    /// <summary>Путь к png-изображению - превью шаблона</summary>
    public string PreviewPath { get; init; }
}

/// <summary>
/// Шаблон приглашения (информация для обновления)
/// </summary>
/// <remarks>
/// Все поля, кроме ID, необязательны для заполнения - их нужно заполнять, если нужно обновить значение.
///<para></para>
/// Если нужно оставить значение без изменения - передать null
/// </remarks>
public record TemplateUpdateDto
{
    /// <summary>
    /// Шаблон приглашения (информация для обновления)
    /// </summary>
    /// <param name="id">ID шаблона</param>
    /// <param name="price">Цена</param>
    /// <param name="name">Название</param>
    /// <param name="idEvent">ID типа мероприятия</param>
    /// <param name="filePath">Путь к html-файлу шаблона</param>
    /// <param name="previewPath">Путь к изображению - превью шаблона</param>
    public TemplateUpdateDto(int id, int? price = null, string? name = null, int? idEvent = null,
        string? filePath = null, string? previewPath = null)
    {
        Id = id;
        Price = price;
        Name = name;
        IdEvent = idEvent;
        FilePath = filePath;
        PreviewPath = previewPath;
    }

    /// <summary>ID шаблона</summary>
    [Required]
    public int Id { get; init; }

    /// <summary>Цена</summary>
    public int? Price { get; init; }

    /// <summary>Название</summary>
    public string? Name { get; init; }

    /// <summary>ID типа мероприятия</summary>
    public int? IdEvent { get; init; }

    /// <summary>Путь к html-файлу шаблона</summary>
    public string? FilePath { get; init; }

    /// <summary>Путь к png-изображению - превью шаблона</summary>
    public string? PreviewPath { get; init; }
}

/// <summary>
/// Шаблон приглашения (информация для добавления)
/// </summary>
public record TemplateCreatingDto
{
    /// <summary>
    /// Шаблон приглашения (информация для добавления)
    /// </summary>
    /// <param name="price">Цена</param>
    /// <param name="name">Название</param>
    /// <param name="idEvent">ID типа мероприятия</param>
    /// <param name="filePath">Путь к html-файлу шаблона</param>
    /// <param name="previewPath">Путь к изображению - превью шаблона</param>
    public TemplateCreatingDto(int price, string name, int idEvent, string filePath, string previewPath)
    {
        Price = price;
        Name = name;
        IdEvent = idEvent;
        FilePath = filePath;
        PreviewPath = previewPath;
    }

    /// <summary>Цена</summary>
    public int Price { get; init; }

    /// <summary>Название</summary>
    public string Name { get; init; }

    /// <summary>ID типа мероприятия</summary>
    public int IdEvent { get; init; }

    /// <summary>Путь к html-файлу шаблона</summary>
    public string FilePath { get; init; }

    /// <summary>Путь к изображению - превью шаблона</summary>
    public string PreviewPath { get; init; }
}