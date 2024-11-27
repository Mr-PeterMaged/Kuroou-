using Ecommerce.Api.Extentions;
using Ecommerce.Api.Middlewares;
using Ecommerce.Core.Entities.Identity;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructre.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Microsoft.AspNetCore.Identity;
using Ecommerce.Infrastructre.Data.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var secuirtySchema = new OpenApiSecurityScheme
    {
        Description = "JWt Auth Bearer Schema",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", secuirtySchema);
    var securityRequirements = new OpenApiSecurityRequirement
    {
        {secuirtySchema, new[]{"Bearer"} }
    };
    c.AddSecurityRequirement(securityRequirements);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(opt =>
       {
           opt.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Token:Key").Value)),
               ValidIssuer = builder.Configuration.GetSection("Token:Issuer").Value,
               ValidateIssuer = true,
               ValidateAudience = false

		   };
       });

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
	builder.AllowAnyOrigin()
		   .AllowAnyMethod()
		   .AllowAnyHeader();
}));

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder
       .Services
       .AddDbContext<StoreContext>
       (opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder
       .Services
       .AddIdentity<AppUser, IdentityRole>()
       .AddEntityFrameworkStores<StoreContext>();


builder.Services.AddApplicationServices();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IConnectionMultiplexer, ConnectionMultiplexer>(opt =>
{
	var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"), true);
	return ConnectionMultiplexer.Connect(configuration);
});


var app = builder.Build();

app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseMiddleware<ExceptionMiddleware>();
app.UseStaticFiles();


using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var logger = service.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
		var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        await AppIdentitySeeding.SeedUserAsync(userManager);
        await context.Database.MigrateAsync();

	}
    catch(Exception ex)
    {
        var loggerFactory = logger.CreateLogger<Program>();
        loggerFactory.LogError(ex, "an error occured");
    }
    
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyPolicy");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
