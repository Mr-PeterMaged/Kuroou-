namespace Ecommerce.Api.Errors
{
	public class ApiResponse
	{
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
			Message = message ?? GetDefaultMessageForStatusCode(statusCode);
		}

		private string GetDefaultMessageForStatusCode(int statusCode)
		{
			return StatusCode switch
			{
				400 => "A bad request you have made",
				401 => "Authorization, you are not authorized",
				404 => "Resource Found It Was Not",
				500 => "Un Expected Error Suden in the server",
				_ => default
			};
		}
	}
}
