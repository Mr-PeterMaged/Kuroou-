using Ecommerce.Api.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
	[Route("errors/{code}")]
	[ApiExplorerSettings(IgnoreApi = true)]
	public class ErrorController : BaseController
	{
		public IActionResult Error(int code)
		{
			return new ObjectResult(new ApiResponse(code));
		}
	}
}
