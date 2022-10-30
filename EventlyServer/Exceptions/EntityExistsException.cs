namespace EventlyServer.Exceptions;

/// <summary>
/// Исключение обозначает, что объект с такими характеристиками уже сущестует и второй не может быть создан
/// </summary>
public class EntityExistsException : Exception
{
    public EntityExistsException(string? message) : base(message)
    {
    }
}