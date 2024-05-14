using System.Collections.Generic;

namespace BlazorShared.Models;
public class OrderDetail : Order
{
    public Address ShipToAdress { get; set; } = new Address();
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
}
