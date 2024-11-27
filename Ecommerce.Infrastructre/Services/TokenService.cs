
using Ecommerce.Core.Entities.Identity;
using Ecommerce.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Infrastructre.Services
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration _config;
		private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration configuration)
        {
			_config = configuration;
			_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
		}
        public string CreateToken(AppUser appUser)
		{
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
				new Claim(JwtRegisteredClaimNames.GivenName, appUser.DisplayName)
			};
			var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
			var tokenDiscrptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(7),
				SigningCredentials = creds,
				Issuer = _config["Token:Issuer"]
			};
			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDiscrptor);

			return tokenHandler.WriteToken(token);

		}
	}
}
