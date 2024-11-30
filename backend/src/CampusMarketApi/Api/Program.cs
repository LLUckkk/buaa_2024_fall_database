using CampusMarketApi.Infrastructure.Services.Auth;
using CampusMarketApi.Core.Interfaces.Auth;
using CampusMarketApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CampusMarketApi.Core.Interfaces;
using CampusMarketApi.Infrastructure.Services;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using CampusMarketApi.Core.Models.Requests;
using CampusMarketApi.Core.Models.Responses;

namespace CampusMarketApi.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateSlimBuilder(args);

			builder.Services.ConfigureHttpJsonOptions(options =>
				{
				});

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(c =>
			{
				c.EnableAnnotations();
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
				c.ExampleFilters();
			});
			
			builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = builder.Configuration["Jwt:Issuer"],
						ValidAudience = builder.Configuration["Jwt:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
					};
				});

			builder.Services.AddAuthorization();

			builder.Services.AddScoped<IAppLogger<string>, AppLogger>();
			builder.Services.AddScoped<ITokenService, TokenService>();
			builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
			builder.Services.AddScoped<IUserService, UserService>();

			builder.Services.AddSwaggerExamplesFromAssemblyOf<RegisterRequestExample>();
			builder.Services.AddSwaggerExamplesFromAssemblyOf<RegisterResponseExample>();
			builder.Services.AddSwaggerExamplesFromAssemblyOf<LoginRequestExample>();
			builder.Services.AddSwaggerExamplesFromAssemblyOf<LoginResponseExample>();

			var app = builder.Build();
			app.MapControllers();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.Run();
		}
	}
}
