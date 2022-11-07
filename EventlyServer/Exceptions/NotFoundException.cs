namespace EventlyServer.Exceptions;

/// <summary>
/// Исключение обозначает, что необходимый объект не был найден
/// </summary>
public class EntityNotFoundException : Exception
{
    /// <summary>
    /// Создание исключения
    /// </summary>
    /// <param name="name">Название сущности</param>
    /// <param name="key">Конфликтующее значение</param>
    public EntityNotFoundException(string name, object? key)
        : base($"Entity <{name}> ({key ?? "null"}) not found")
    {
    }
}