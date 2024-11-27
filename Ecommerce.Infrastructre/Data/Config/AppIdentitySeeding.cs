
using Ecommerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructre.Data.Config
{
	public static class AppIdentitySeeding
	{
		public static async Task SeedUserAsync(UserManager<AppUser> userManager)
		{
			if (!userManager.Users.Any())
			{
				var user = new AppUser
				{
					DisplayName = "Mohamed",
					Email = "Mohamed@test.com",
					UserName = "Mohamed@test.com",
					Address = new Address
					{
						FirstName = "Mohamed",
						LastName = "Hassan",
						Street = "Alshall Street",
						State = "Cairo",
						ZipCode = "1234",
						City = "Cairo"
					}
				};

				await userManager.CreateAsync(user, "P@ssw0rd");
			}
		}
	}
}
