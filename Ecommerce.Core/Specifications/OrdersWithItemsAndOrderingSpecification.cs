
using Ecommerce.Core.Entities.OrderAggregate;

namespace Ecommerce.Core.Specifications
{
	public class OrdersWithItemsAndOrderingSpecification : BaseSpecification<Order>
	{
		public OrdersWithItemsAndOrderingSpecification(string email)
						: base(o => o.BuyerEmail == email)
		{
			AddInclude(x => x.OrderItems);
			AddInclude(x => x.DeliveryMethod);
			AddInclude(x => x.OrderDate);
		}

		public OrdersWithItemsAndOrderingSpecification(int id, string emailBuyer)
			: base(o => o.Id == id && o.BuyerEmail == emailBuyer)
		{
			AddInclude(x => x.OrderItems);
			AddInclude(x => x.DeliveryMethod);
		}
	}
}
