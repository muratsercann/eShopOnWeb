using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Specifications;
public class OrderPaginatedSpecification
{
    [Fact]
    public void ReturnAllOrders()
    {
        var spec = new eShopWeb.ApplicationCore.Specifications.OrderPaginatedSpecification(0, 10);

        var result = spec.Evaluate(GetTestCollection());

        Assert.NotNull(result);
        Assert.Equal(3, result.ToList().Count);
    }

    private List<Order> GetTestCollection()
    {
        var orderList = new List<Order>();

        Address address = new Address(
                "street-1",
                "city-1",
                "state-1",
                "country-1", "zipcode-1");

        orderList.Add(new Order("1",address,
             new List<OrderItem> {
                 new OrderItem(new CatalogItemOrdered(1,"product-1","TestUri1"),1.00M,1),
                 }
             ));

        orderList.Add(new Order("2", address,
             new List<OrderItem> {
                 new OrderItem(new CatalogItemOrdered(1,"product-1","TestUri1"),1.00M,1),
                 new OrderItem(new CatalogItemOrdered(2,"product-2","TestUri2"),1.00M,1)
                 }
             ));

        orderList.Add(new Order("3", address,
             new List<OrderItem> {
                 new OrderItem(new CatalogItemOrdered(1,"product-1","TestUri1"),1.00M,1),
                 new OrderItem(new CatalogItemOrdered(2,"product-2","TestUri2"),1.00M,1)
                 }
             ));


        return orderList;
    }
}
