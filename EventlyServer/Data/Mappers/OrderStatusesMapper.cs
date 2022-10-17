using EventlyServer.Data.Entities.Enums;

namespace EventlyServer.Data.Mappers;

public static class OrderStatusesMapper
{
    /// <summary>
    /// Преобразовывает строку в enum <c>OrderStatuses</c>
    /// </summary>
    /// <param name="value">конвертируемое значение</param>
    /// <returns>элемент перечисления <c>OrderStatuses</c></returns>
    /// <exception cref="ArgumentException">если переданная строка не может быть сконвертирована в этот enum</exception>
    /// <exception cref="ArgumentNullException">если переданная строка содержит null</exception>
    public static OrderStatuses ToOrderStatuses(this string value)
    {
        return value switch
        {
            nameof(OrderStatuses.ACCEPTED) => OrderStatuses.ACCEPTED,
            nameof(OrderStatuses.IN_PROGRESS) => OrderStatuses.IN_PROGRESS,
            nameof(OrderStatuses.DONE) => OrderStatuses.DONE,
            nameof(OrderStatuses.ONLINE) => OrderStatuses.ONLINE,
            nameof(OrderStatuses.CANCELED) => OrderStatuses.CANCELED,
            { } => throw new ArgumentException("This value cannot be converted into OrderStatus", nameof(value)),
            null => throw new ArgumentNullException(nameof(value))
        };
    }
}