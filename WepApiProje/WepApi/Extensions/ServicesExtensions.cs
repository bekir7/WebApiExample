using AspNetCoreRateLimit;
using Entities.DataTransferObject;
using Entities.Models;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presentation.ActionFilters;
using Presentation.Controllers;
using Repositories.Contracts;
using Repositories.EFCore;
using Services;
using Services.Contracts;
using System.Data.SqlTypes;
using System.Text;

namespace WepApi.Extensions
{
	public static class ServicesExtensions
	{
		public static void ConfigureSqlContext(this IServiceCollection services,IConfiguration configuration)=>
		    services.AddDbContext<RepositoryContext>(options => 
			options.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
		
		public static void ConfigureRepositoryManager(this IServiceCollection services)=>
				services.AddScoped<IRepositoryManager,RepositoryManager>();
		
		public static void ConfigureServiceManager(this IServiceCollection services)=>
			services.AddScoped<IServiceManager,ServiceManager>();
	
	    public static void ConfigureLoggerService(this IServiceCollection services)=>
			services.AddSingleton<ILoggerService,LoggerManager>();
	
		public static void ConfigureActionFilters(this IServiceCollection services)
		{
			services.AddScoped<ValidationAttributeFilter>();//IoC
			services.AddSingleton<LogFilterAttribute>();
			services.AddScoped<ValidateMediaTypeAttribute>();
		}

		public static void ConfigureCors(this IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy", builder =>
				builder.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader()
				.WithExposedHeaders("X-Pagination")
				);
			});
		}

		public static void ConfigureDataShaper(this IServiceCollection services)
		{
			services.AddScoped<IDataShaper<BookDto>,DataShaper<BookDto>>();	
		}

		public static void AddCustomMediaTypes(this IServiceCollection services)
		{
			services.Configure<MvcOptions>(config =>
			{
				var systemTextJsonOutputFormatter = config
				.OutputFormatters
				.OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();


				if (systemTextJsonOutputFormatter != null)
				{
					systemTextJsonOutputFormatter.SupportedMediaTypes
					.Add("application/vnd.anan.hateoas+json");

					systemTextJsonOutputFormatter.SupportedMediaTypes
					.Add("application/vnd.anan.apiroot+json");
				}
				var xmlOutputFormatter = config
				.OutputFormatters
				.OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();


				if (xmlOutputFormatter != null)
				{
					xmlOutputFormatter.SupportedMediaTypes
					.Add("application/vnd.anan.hateoas+xml");

					xmlOutputFormatter.SupportedMediaTypes
					.Add("application/vnd.anan.apiroot+xml");
				}
			});
		}
		public static void ConfigureVersioning(this IServiceCollection services)
		{
			services.AddApiVersioning(opt =>
			{
				opt.ReportApiVersions = true;
				opt.AssumeDefaultVersionWhenUnspecified = true;
				opt.DefaultApiVersion = new ApiVersion(1, 0);
				opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
				opt.Conventions.Controller<BooksController>()
				.HasApiVersion(new ApiVersion(1, 0));

				opt.Conventions.Controller<Booksv2Controller>()
				.HasDeprecatedApiVersion(new ApiVersion(2, 0));
			});
		}
		public static void ConfigureResponseCaching(this IServiceCollection services) =>
               services.AddResponseCaching();

		public static void ConfigureHttpCacheHeaders(this IServiceCollection services)=>
			services.AddHttpCacheHeaders(expirationOpt =>
			{
				expirationOpt.MaxAge = 90;
				expirationOpt.CacheLocation = CacheLocation.Private;
			},
			validationOpt=>
			{
				validationOpt.MustRevalidate = false;
			});

		public static void ConfigureRateLimitOptions(this IServiceCollection services)
		{
			var rateLimitRules = new List<RateLimitRule>()
			{
				new RateLimitRule()
				{
					Endpoint="*",
					Limit=60,
					Period="1m"
				}
			};
			services.Configure<IpRateLimitOptions>(opt =>
			{
				opt.GeneralRules = rateLimitRules;
			});
			services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
			services.AddSingleton<IIpPolicyStore,MemoryCacheIpPolicyStore>();
			services.AddSingleton<IRateLimitConfiguration,RateLimitConfiguration>();
			services.AddSingleton<IProcessingStrategy,AsyncKeyLockProcessingStrategy>();
		}

		public static void ConfigureIdentity(this IServiceCollection services)
		{
			var builder = services.AddIdentity<User, IdentityRole>(opt =>
			{
				opt.Password.RequireDigit = true;
				opt.Password.RequireLowercase = false;
				opt.Password.RequireUppercase = false;
				opt.Password.RequireNonAlphanumeric = false;
				opt.Password.RequiredLength = 8;
				opt.User.RequireUniqueEmail = true;
			})
			.AddEntityFrameworkStores<RepositoryContext>()
			.AddDefaultTokenProviders();
		}
		public static void ConfigureJwt(this IServiceCollection services,IConfiguration configuration)
		{
			var jwtSettings = configuration.GetSection("JwtSetting");
			var secretKey = jwtSettings["secretKey"];

			services.AddAuthentication(opt =>
			{
				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

			}).AddJwtBearer(opts =>
			{
				opts.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = jwtSettings["validIssuer"],
					ValidAudience = jwtSettings["validAudience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
				};
			});
		}
		public static void ConfigureSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(s =>
			{
				s.SwaggerDoc("v1",new OpenApiInfo { Title="ANAN",Version="v1",Description="ANAN ASP.NET Core Web Api"});//daha fazla özellik istersen btk kurs 27.bölüm 4.video
				s.SwaggerDoc("v2",new OpenApiInfo { Title="ANAN",Version="v2"});

				s.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme
				{
					In=ParameterLocation.Header,
					Description="Place to add JWT with Bearer",
					Name="Authorization",
					Type=SecuritySchemeType.ApiKey,
					Scheme="Bearer"
				});
				s.AddSecurityRequirement(new OpenApiSecurityRequirement()
				{
					{
						new OpenApiSecurityScheme
						{
							Reference=new OpenApiReference
							{
								Type=ReferenceType.SecurityScheme,
								Id="Bearer"
							},
							Name="Bearer"
						},
						new List<string>()
					}
				});
			});
		}
		public static void RegisterRepository(this IServiceCollection services)
		{
			services.AddScoped<IBookRepository, BookRepository>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();

		}
		public static void RegisterService(this IServiceCollection services)
		{
			services.AddScoped<IBookService, BookManager>();
			services.AddScoped<ICategoryService, CategoryManager>();
			services.AddScoped<IAuthenticationService, AuthenticationManager>();

		}
	}
}
