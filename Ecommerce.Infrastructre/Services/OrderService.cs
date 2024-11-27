
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.OrderAggregate;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Specifications;

namespace Ecommerce.Infrastructre.Services
{
	public class OrderService : IOrederService
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IUnitOfWork _unitOfWork;

		public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
			_basketRepository = basketRepository;
			_unitOfWork = unitOfWork;
		}
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, OrderAddress orderAddress)
		{
			var basket = await _basketRepository.GetBasketAsync(basketId);
			var items = new List<OrderItem>();
			foreach (var item in basket.Items)
			{
				var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
				var itemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
				var orderItem = new OrderItem(itemOrdered, product.Price, item.Quantity);
				items.Add(orderItem);
			}
			var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
			var subTotal = items.Sum(item => item.Price * item.Quantity);

			var order = new Order(buyerEmail, orderAddress, deliveryMethod, items, subTotal);

			_unitOfWork.Repository<Order>().Add(order);

			var result = await _unitOfWork.Complete();

			if (result <= 0) return null;

			await _basketRepository.DeleteBasketAsync(basketId);
			return order;
		}

		public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
		{
			return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
		}

		public async Task<Order> GetOrderByUserIdAsync(int id, string buyerEmail)
		{
			var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);
			return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
		}

		public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
		{
			var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);
			return await _unitOfWork.Repository<Order>().ListAsync(spec);
		}
	}
}
