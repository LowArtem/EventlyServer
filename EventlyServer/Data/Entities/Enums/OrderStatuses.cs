namespace EventlyServer.Data.Entities.Enums;

/// <summary>
/// Возможные варианты статуса заказа
/// </summary>
public enum OrderStatuses
{
    ACCEPTED,
    IN_PROGRESS,
    DONE,
    ONLINE,
    CANCELED
}