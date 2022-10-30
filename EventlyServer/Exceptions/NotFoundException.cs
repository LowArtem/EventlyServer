namespace EventlyServer.Exceptions;

/// <summary>
/// Исключение обозначает, что необходимый объект не был найден
/// </summary>
public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string? message) : base(message)
    {
    }
}