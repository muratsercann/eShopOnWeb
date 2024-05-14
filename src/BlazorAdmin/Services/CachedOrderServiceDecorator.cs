using System.Collections.Generic;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorShared.Interfaces;
using Microsoft.Extensions.Logging;
using BlazorShared.Models;
using System;

namespace BlazorAdmin.Services;

public class CachedOrderServiceDecorator : IOrderService
{
    private readonly ILocalStorageService _localStorageService;
    private readonly OrderService _orderService;
    private ILogger<CachedOrderServiceDecorator> _logger;

    public CachedOrderServiceDecorator(ILocalStorageService localStorageService,
        OrderService orderservice,
        ILogger<CachedOrderServiceDecorator> logger)
    {
        _localStorageService = localStorageService;
        _orderService = orderservice;
        _logger = logger;
    }

   
    public async Task<List<Order>> List()
    {
        string key = "orders";
        var cacheEntry = await _localStorageService.GetItemAsync<CacheEntry<List<Order>>>(key);
        if (cacheEntry != null)
        {
            _logger.LogInformation("Loading orders from local storage.");
            if (cacheEntry.DateCreated.AddMinutes(1) > DateTime.UtcNow)
            {
                return cacheEntry.Value;
            }
            else
            {
                _logger.LogInformation($"Loading {key} from local storage.");
                await _localStorageService.RemoveItemAsync(key);
            }
        }

        var items = await _orderService.List();
        var entry = new CacheEntry<List<Order>>(items);
        await _localStorageService.SetItemAsync(key, entry);
        return items;
    }

    public async Task<List<Order>> ListPaged(int pageSize)
    {
        string key = "orders";
        var cacheEntry = await _localStorageService.GetItemAsync<CacheEntry<List<Order>>>(key);
        if (cacheEntry != null)
        {
            _logger.LogInformation("Loading orders from local storage.");
            if (cacheEntry.DateCreated.AddMinutes(1) > DateTime.UtcNow)
            {
                return cacheEntry.Value;
            }
            else
            {
                _logger.LogInformation($"Loading {key} from local storage.");
                await _localStorageService.RemoveItemAsync(key);
            }
        }

        var items = await _orderService.ListPaged(pageSize);
        var entry = new CacheEntry<List<Order>>(items);
        await _localStorageService.SetItemAsync(key, entry);
        return items;
    }

    public async Task<OrderDetail> GetDetails(int orderId)
    {
        string key = $"orderdetail-{orderId}";
        var cacheEntry = await _localStorageService.GetItemAsync<CacheEntry<OrderDetail>>(key);
        if (cacheEntry != null)
        {
            _logger.LogInformation($"Loading {key} from local storage.");
            if (cacheEntry.DateCreated.AddMinutes(1) > DateTime.UtcNow)
            {
                return cacheEntry.Value;
            }
            else
            {
                _logger.LogInformation($"Loading {key} from local storage.");
                await _localStorageService.RemoveItemAsync(key);
            }
        }

        var order = await _orderService.GetDetails(orderId);
        var entry = new CacheEntry<OrderDetail>(order);
        await _localStorageService.SetItemAsync(key, entry);
        return order;
    }

   
    private async Task RefreshLocalStorageList()
    {
        string key = "orders";

        await _localStorageService.RemoveItemAsync(key);
        var items = await _orderService.List();
        var entry = new CacheEntry<List<Order>>(items);
        await _localStorageService.SetItemAsync(key, entry);
    }

}
