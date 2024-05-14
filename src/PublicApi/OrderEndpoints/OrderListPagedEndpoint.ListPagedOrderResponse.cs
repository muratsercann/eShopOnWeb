using System.Collections.Generic;
using System;
using BlazorShared.Models;

namespace Microsoft.eShopWeb.PublicApi.OrderEndpoints;

public class ListPagedOrderResponse : BaseResponse
{
    public ListPagedOrderResponse(Guid correlationId) : base(correlationId)
    {
    }

    public ListPagedOrderResponse()
    {
    }

    public List<Order> Orders { get; set; } = new List<Order>();
    public int PageCount { get; set; }
}
