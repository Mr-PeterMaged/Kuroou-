using Ecommerce.Api.Errors;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructre.Data;
using Ecommerce.Infrastructre.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Extentions
{
	public static class ApplicationServceExtention
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped<IResponseCacheService, ResponseCacheService>();
			services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			services.AddScoped<IBasketRepository, BasketRepository>();
			services.AddScoped<ITokenService, TokenService>();
			services.AddScoped<IOrederService, OrderService>();
			
			services.Configure<ApiBehaviorOptions>(opt =>
			{
				opt.InvalidModelStateResponseFactory = actionContext =>
				{
					var errors = actionContext
											  .ModelState
											  .Where(e => e.Value.Errors.Count > 0)
											  .SelectMany(x => x.Value.Errors)
											  .Select(x => x.ErrorMessage)
											  .ToArray();
					var errorResponse = new ApiValidationErrorResponse
					{
						Errors = errors
					};
					return new BadRequestObjectResult(errorResponse);
				};
			});
			return services;
		}
	}
}
