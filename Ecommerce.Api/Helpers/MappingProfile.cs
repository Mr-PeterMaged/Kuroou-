using AutoMapper;
using Ecommerce.Api.Dtos;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.Identity;
using Ecommerce.Core.Entities.OrderAggregate;

namespace Ecommerce.Api.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(p => p.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(p => p.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(p => p.PictureUrl, o => o.MapFrom<ProductUrlResolver>());

            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();

			CreateMap<AddressDto, OrderAddress>().ReverseMap();
			CreateMap<Order, OrderToReturnDto>()
				.ForMember(x => x.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
				.ForMember(x => x.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));
			CreateMap<OrderItem, OrderItemDto>()
				.ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
				.ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
				.ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
				.ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());

		}
    }
}
