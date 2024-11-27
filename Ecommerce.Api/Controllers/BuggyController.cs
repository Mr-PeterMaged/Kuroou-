using Ecommerce.Api.Errors;
using Ecommerce.Infrastructre.Data;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
	public class BuggyController : BaseController
	{
        private readonly StoreContext _storeContext;

		public BuggyController(StoreContext context)
        {
			_storeContext = context;

		}
		[HttpGet("notfound")]
		public ActionResult GetNotFoundRequest()
		{
			var thing = _storeContext.Product.Find(88);
			if (thing == null)
			{
				return NotFound(new ApiResponse(404));
			}
			return Ok();
		}

		[HttpGet("servererror")]
		public ActionResult GetServerError()
		{
			var thing = _storeContext.Product.Find(42);
			var toReturn = thing.ToString();
			return Ok(new ApiResponse(500));
		}

		[HttpGet("badrequest")]
		public ActionResult GetBadRequest()
		{
			return BadRequest(new ApiResponse(400));
		}

		[HttpGet("unauthorize")]
		public ActionResult GetUnauthorizeRequest()
		{
			return Unauthorized(new ApiResponse(401));
		}
	}
}
