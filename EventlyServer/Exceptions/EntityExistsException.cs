namespace EventlyServer.Exceptions;

/// <summary>
/// Исключение обозначает, что объект с такими характеристиками уже сущестует и второй не может быть создан
/// </summary>
public class EntityExistsException : Exception
{
    /// <summary>
    /// Создание исключения
    /// </summary>
    /// <param name="name">Название сущности</param>
    /// <param name="key">Конфликтующее значение</param>
    public EntityExistsException(string name, object? key)
        : base($"Entity <{name}> ({key ?? "null"}) already exists")
    {
    }
}