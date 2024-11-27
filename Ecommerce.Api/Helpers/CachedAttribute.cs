using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace Ecommerce.Api.Helpers
{
	public class CachedAttribute : Attribute, IAsyncActionFilter
	{
		private readonly int _timeToLiveSeconds;

		public CachedAttribute(int timeToLiveSeconds)
        {
			_timeToLiveSeconds = timeToLiveSeconds;
		}
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var cachedService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
			var cachedKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
			var cachedResponse = await cachedService.GetCachedResponseAsync(cachedKey);
			if (!string.IsNullOrEmpty(cachedResponse))
			{
				var contentResult = new ContentResult
				{
					Content = cachedResponse,
					ContentType = "application/json",
					StatusCode = 200,
				};
				context.Result = contentResult;
				return;
			}
			var executedContext = await next();
			if (executedContext.Result is OkObjectResult okObject) 
			{
				await cachedService.CacheResponseAsync(cachedKey, okObject.Value, TimeSpan.FromSeconds(_timeToLiveSeconds));
			}
		}
		private string GenerateCacheKeyFromRequest(HttpRequest request)
		{
			var keyBuilder = new StringBuilder();
			keyBuilder.Append($"{request.Path}");
			foreach(var (key,value) in request.Query.OrderBy(x => x.Key))
			{
				keyBuilder.Append($"|{key}-{value}");
			}
			return keyBuilder.ToString();
		}
	}
}
