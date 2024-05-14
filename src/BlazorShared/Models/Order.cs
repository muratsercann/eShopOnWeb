using System;

namespace BlazorShared.Models;
public class Order
{
    public int Id { get; set; }

    public DateTimeOffset Date { get; set; }

    public string BuyerId { get; set; }

    public decimal Total {  get; set; }

    public string Status { get; set; } = "Pending";

}
