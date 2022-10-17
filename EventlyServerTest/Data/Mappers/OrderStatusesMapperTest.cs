using EventlyServer.Data.Entities.Enums;
using EventlyServer.Data.Mappers;

namespace EventlyServerTest.Data.Mappers;

public class OrderStatusesMapperTest
{
    [Fact]
    public void ToString_Test()
    {
        var orderStatus = OrderStatuses.DONE;
        string orderStringExp = "DONE";

        string test = orderStatus.ToString();
        Assert.Equal(orderStringExp, test);
    }

    [Fact]
    public void FromString_Test()
    {
        string orderString = "DONE";
        var orderStatusExp = OrderStatuses.DONE;

        var test = orderString.ToOrderStatuses();
        Assert.Equal(orderStatusExp, test);
    }
}