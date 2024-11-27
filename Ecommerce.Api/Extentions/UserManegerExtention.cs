using Ecommerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce.Api.Extentions
{
	public static class UserManegerExtention
	{
		public static async Task<AppUser> FindByEmailWithAddressAsync(this UserManager<AppUser> input,
																      ClaimsPrincipal user)
		{
			var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
			var result = await input.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Email == email);
			return result;
		}

		public static async Task<AppUser> FindByEmailFromClaimsPrinciple(this UserManager<AppUser> input,
																		 ClaimsPrincipal user)
		{
			var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
			var result = await input.Users.SingleOrDefaultAsync(x => x.Email == email);
			return result;
		}
	}
}
