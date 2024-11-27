
using Ecommerce.Core.Entities.Identity;

namespace Ecommerce.Core.Interfaces
{
	public interface ITokenService
	{
		string CreateToken(AppUser appUser);
	}
}
