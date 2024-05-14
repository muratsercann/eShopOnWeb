using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorShared.Models;

namespace BlazorShared.Interfaces;
public interface IOrderService
{
    Task<List<Order>> List();
    Task<List<Order>> ListPaged(int pageSize);
    Task<OrderDetail> GetDetails(int orderId);
}
