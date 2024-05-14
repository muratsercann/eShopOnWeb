using System;
using BlazorShared.Models;

namespace Microsoft.eShopWeb.PublicApi.OrderEndpoints;

public class OrderDetailResponse : BaseResponse
{
    public OrderDetailResponse(Guid correlationId) : base(correlationId)
    {
    }

    public OrderDetailResponse()
    {
    }

    public OrderDetail OrderDetail { get; set; }
}
