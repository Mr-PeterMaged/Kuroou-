
using Ecommerce.Core.Entities.OrderAggregate;

namespace Ecommerce.Core.Interfaces
{
	public interface IOrederService
	{
		Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, OrderAddress orderAddress);
		Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
		Task<Order> GetOrderByUserIdAsync(int id, string buyerEmail);
		Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
	}
}
