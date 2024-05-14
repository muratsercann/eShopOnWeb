using System.Linq;
using System.Threading.Tasks;
using BlazorShared.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using MinimalApi.Endpoint;

namespace Microsoft.eShopWeb.PublicApi.OrderEndpoints;

/// <summary>
/// Get an Order with Details by id
/// </summary>
public class OrderDetailEndpoint : IEndpoint<IResult, OrderDetailRequest, IRepository<ApplicationCore.Entities.OrderAggregate.Order>>
{
    private readonly IUriComposer _uriComposer;

    public OrderDetailEndpoint(IUriComposer uriComposer)
    {
        _uriComposer = uriComposer;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("api/order-detail/{orderId}",
            async (int orderId, IRepository<ApplicationCore.Entities.OrderAggregate.Order> orderRepository) =>
            {
                return await HandleAsync(new OrderDetailRequest(orderId), orderRepository);
            })
            .Produces<OrderDetailResponse>()
            .WithTags("OrderEndpoints");
    }

    public async Task<IResult> HandleAsync(OrderDetailRequest request, IRepository<ApplicationCore.Entities.OrderAggregate.Order> itemRepository)
    {
        var response = new OrderDetailResponse(request.CorrelationId());

        var spec = new OrderWithItemsByIdSpec(request.OrderId);
        var order = await itemRepository.FirstOrDefaultAsync(spec);

        if (order is null)
            return Results.NotFound();

        response.OrderDetail = new OrderDetail
        {
            Id = order.Id,
            BuyerId = order.BuyerId,
            Date = order.OrderDate,
            Items = order.OrderItems.Select(oi => new OrderItem
            {
                PictureUrl = oi.ItemOrdered.PictureUri,
                ProductId = oi.ItemOrdered.CatalogItemId,
                ProductName = oi.ItemOrdered.ProductName,
                UnitPrice = oi.UnitPrice,
                Units = oi.Units
            }).ToList(),
            Total = order.Total(),
            ShipToAdress = new Address()
            {
                Street = order.ShipToAddress.Street,
                City = order.ShipToAddress.City,
                State = order.ShipToAddress.State,
                Country = order.ShipToAddress.Country,
                ZipCode = order.ShipToAddress.ZipCode
            }
        };

        return Results.Ok(response);
    }
}
