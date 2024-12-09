using System.Text;
using Market.Interfaces;
using Market.Services;
using Market.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Market
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.ConfigureHttpJsonOptions(options =>
			{
			});

			builder.Services.AddCors(options =>
			{
				options.AddDefaultPolicy(builder =>
				{
					builder.AllowAnyOrigin()
						.AllowAnyHeader()
						.AllowAnyMethod();
				});
			});
			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(c =>
			{
				c.EnableAnnotations();
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Campus Market", Version = "v114" });
				c.ExampleFilters();
			});
			builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();
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
				})
				.AddCookie(
					CookieAuthenticationDefaults.AuthenticationScheme,
					options => builder.Configuration.Bind("CookieSettings", options)
				);

			builder.Services.AddHttpContextAccessor();

			builder.Services.AddSingleton<IAppLogger<string>, AppLogger>();
			builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
			builder.Services.AddSingleton<ITokenService, TokenService>();

			builder.Services.AddScoped<IChatService, ChatService>();
			builder.Services.AddScoped<ICommentService, CommentService>();
			builder.Services.AddScoped<IFileService, FileService>();
			builder.Services.AddScoped<IPaymentService, PaymentService>();
			builder.Services.AddScoped<IProductCollectService, ProductCollectService>();
			builder.Services.AddScoped<IProductInfoService, ProductInfoService>();
			builder.Services.AddScoped<IProductOrderService, ProductOrderService>();
			builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
			builder.Services.AddScoped<IProductCollectService, ProductCollectService>();
			builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
			builder.Services.AddScoped<ISystemUserService, SystemUserService>();
			builder.Services.AddScoped<IUserAddressService, UserAddressService>();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IVoucherService, VoucherService>();

			builder.Services.AddHostedService<ClearOldTokenTask>();
			builder.Services.AddHostedService<SaveDatabaseChangesTask>();
			builder.Services.AddHostedService<TimeoutTasks>();
			builder.Services.AddAuthorization();

			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
