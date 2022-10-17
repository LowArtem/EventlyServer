namespace EventlyServer.Data.Entities.Abstract;

/// <summary>
/// Базовый класс всех сущностей базы данных, наделяет все сущности параметром id.
/// <para>
/// Необходим для правильной работы репозиториев: репозиторий может параметризовываться только классом Entity
/// </para>
/// <remarks>Сущности-посредники в связи многие-ко-многим НЕ могут быть наследниками этого класса и иметь репозиторий</remarks>
/// </summary>
public abstract class Entity
{
    public int Id { get; set; }
}