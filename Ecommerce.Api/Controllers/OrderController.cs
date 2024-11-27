
using AutoMapper;
using Ecommerce.Api.Dtos;
using Ecommerce.Api.Errors;
using Ecommerce.Api.Extentions;
using Ecommerce.Core.Entities.OrderAggregate;
using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
	[Authorize(AuthenticationSchemes = "Bearer")]
	public class OrderController : BaseController
	{
		private readonly IOrederService _orderService;
		private readonly IMapper _mapper;
        public OrderController(IOrederService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

		[HttpPost]
		public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
		{
			var email = HttpContext.User.RetrieveEmailFromPrincipl();
			var address = _mapper.Map<AddressDto, OrderAddress>(orderDto.ShipToAddress);
			var order = await _orderService.CreateOrderAsync(email,
															 orderDto.DeliveryMethodId,
															 orderDto.BasketId,
															 address);
			if (order == null) return BadRequest(new ApiResponse(400,
																"problem with creating order"));
			return Ok(order);
		}

		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
		{
			var email = HttpContext.User.RetrieveEmailFromPrincipl();
			var orders = await _orderService.GetOrdersForUserAsync(email);

			return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
		{
			var email = HttpContext.User.RetrieveEmailFromPrincipl();
			var order = await _orderService.GetOrderByUserIdAsync(id, email);
			if (order == null) return BadRequest(new ApiResponse(404));
			return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
		}

		[HttpGet("delivery-methods")]
		public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
		{
			return Ok(await _orderService.GetDeliveryMethodsAsync());
		}
	}
}
