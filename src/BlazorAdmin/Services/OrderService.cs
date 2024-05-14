using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorShared.Interfaces;
using BlazorShared.Models;
using Microsoft.Extensions.Logging;


namespace BlazorAdmin.Services;

public class OrderService : IOrderService
{
    private readonly HttpService _httpService;
    private readonly ILogger<OrderService> _logger;

    public OrderService(HttpService httpService,
       ILogger<OrderService> logger)
    {
        _httpService = httpService;
        _logger = logger;
    }


    public async Task<List<Order>> List()
    {
        _logger.LogInformation("Fetching all-orders from API.");
        return (await _httpService.HttpGet<PagedOrderResponse>($"all-orders")).Orders;
    }

    public async Task<List<Order>> ListPaged(int pageSize)
    {
        _logger.LogInformation("Fetching all-orders from API.");
        return (await _httpService.HttpGet<PagedOrderResponse>($"all-orders?PageSize={pageSize}")).Orders;
    }

    public async Task<OrderDetail> GetDetails(int orderId)
    {
        _logger.LogInformation($"Fetching order-detail/{orderId} from API.");
        var result = (await _httpService.HttpGet<OrderDetailResponse>($"order-detail/{orderId}")).OrderDetail;
        return result;
    }

}
