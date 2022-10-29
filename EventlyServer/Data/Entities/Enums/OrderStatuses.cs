namespace EventlyServer.Data.Entities.Enums;

/// <summary>
/// Возможные варианты статуса заказа
/// </summary>
public enum OrderStatuses
{
    /// <summary>Заказ принят</summary>
    ACCEPTED,
    /// <summary>Заказ в разработке</summary>
    IN_PROGRESS,
    /// <summary>Заказ выполнен</summary>
    DONE,
    /// <summary>Заказ (лэндинг) онлайн - загружен на сервер и работает</summary>
    ONLINE,
    /// <summary>Заказ отменен</summary>
    CANCELED
}