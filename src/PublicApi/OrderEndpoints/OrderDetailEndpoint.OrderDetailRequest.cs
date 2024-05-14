namespace Microsoft.eShopWeb.PublicApi.OrderEndpoints;

public class OrderDetailRequest : BaseRequest
{
    public int OrderId { get; init; }

    public OrderDetailRequest(int orderId)
    {
        OrderId = orderId;
    }
}
