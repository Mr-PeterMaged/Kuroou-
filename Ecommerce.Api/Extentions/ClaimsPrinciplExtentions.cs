using System.Security.Claims;

namespace Ecommerce.Api.Extentions
{
	public static class ClaimsPrinciplExtentions
	{
		public static string RetrieveEmailFromPrincipl(this ClaimsPrincipal user)
		{
			return user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value!;
		}
	}
}
