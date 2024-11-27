using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.Dtos
{
	public class BasketItemDto
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public string ProductName { get; set; }
		[Required]
		[Range(0.1,double.MaxValue, ErrorMessage ="Price Should Greater Than Zero")]
		public decimal Price { get; set; }
		[Required]
		[Range(0.1, double.MaxValue, ErrorMessage = "Quantity must Greater Than One")]
		public int Quantity { get; set; }
		[Required]
		public string PictureUrl { get; set; }
		[Required]
		public string Type { get; set; }
		[Required]
		public string Brand { get; set; }
	}
}
