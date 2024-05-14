using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using MinimalApi.Endpoint;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using System;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;


namespace Microsoft.eShopWeb.PublicApi.OrderEndpoints;

public class OrderListPagedEndpoint : IEndpoint<IResult, ListPagedOrderRequest, IRepository<Order>>
{
    private readonly IUriComposer _uriComposer;
    private readonly IMapper _mapper;

    public OrderListPagedEndpoint(IUriComposer uriComposer, IMapper mapper)
    {
        _uriComposer = uriComposer;
        _mapper = mapper;
    }


    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("api/all-orders",
            [Authorize(Roles = BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        async (int? pageSize, int? pageIndex, IRepository<Order> orderRepository) =>
            {
                return await HandleAsync(new ListPagedOrderRequest(pageSize, pageIndex), orderRepository);
            })
            .Produces<ListPagedOrderRequest>()
            .WithTags("OrderEndpoints");
    }

    public async Task<IResult> HandleAsync(ListPagedOrderRequest request, IRepository<Order> orderRepository)
    {
        await Task.Delay(1000);
        var response = new ListPagedOrderResponse(request.CorrelationId());

        int totalItems = await orderRepository.CountAsync();

        var pagedSpec = new OrderPaginatedSpecification(
            skip: request.PageIndex * request.PageSize,
            take: request.PageSize);

        var items = await orderRepository.ListAsync(pagedSpec);


        response.Orders.AddRange(items.Select(o => new BlazorShared.Models.Order
        {
            Date = o.OrderDate,
            Id = o.Id,
            BuyerId = o.BuyerId,
            Total = o.Total()
        }));

        response.PageCount = CalculatePageCount(totalItems, request.PageSize);

        return Results.Ok(response);
    }

    int CalculatePageCount(int totalItems, int pageSize)
    {
        int pageCount;

        if (pageSize > 0)
        {
            pageCount = int.Parse(Math.Ceiling((decimal)totalItems / pageSize).ToString());
        }

        else
        {
            pageCount = totalItems > 0 ? 1 : 0;
        }

        return pageCount;
    }
}
