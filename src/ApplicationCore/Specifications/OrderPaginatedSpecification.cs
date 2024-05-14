using System.Linq;
using Ardalis.Specification;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;

namespace Microsoft.eShopWeb.ApplicationCore.Specifications;
public class OrderPaginatedSpecification : Specification<Order>
{
    public OrderPaginatedSpecification(int skip, int take)
        : base()
    {
        if (take == 0)
        {
            take = int.MaxValue;
        }

        Query.Skip(skip).Take(take)
            .Include(o => o.OrderItems);
    }

}
