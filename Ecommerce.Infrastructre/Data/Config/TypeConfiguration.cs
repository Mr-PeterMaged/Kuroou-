
using Ecommerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructre.Data.Config
{
	public class TypeConfiguration : IEntityTypeConfiguration<ProductType>
	{
		public void Configure(EntityTypeBuilder<ProductType> builder)
		{
			builder.HasData(
			new ProductType { Id = 1, Name = "Boards" },
			new ProductType { Id = 2, Name = "Hats" },
			new ProductType { Id = 3, Name = "Boots" },
			new ProductType { Id = 4, Name = "Gloves" }
			);
		}
	}
}
