using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.Dtos
{
	public class RegisterDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string DisplayName { get; set; }
		[Required]
		[RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$",
		ErrorMessage = "Password Must Have 1 UpperCase, 1 LowerCase, 1 number, 1 non alphanumeric and at least 6 character")]
		public string Password { get; set; }
	}
}
