namespace BlazorShared.Models;
public class OrderItem
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount => 0;
    public int Units { get; set; }
    public string? PictureUrl { get; set; }

    public decimal Price { 
        get {
            return UnitPrice * Units;
        } 
    }

}
