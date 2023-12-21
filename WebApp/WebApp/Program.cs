using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Reflection;
using System.Text;
using WebApp.Common.Configurations;
using WebApp.Data;
using WebApp.Data.Entities;
using WebApp.Filters;
using WebApp.Middleware;
using WebApp.Models.Validators;
using WebApp.Services.Implementations;
using WebApp.Services.Interfaces;


internal class Program
{
    [Obsolete]
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureAppSettings(builder.Host, builder.Environment);
        AddAppSettings(builder.Services, builder.Configuration);
        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();
        ConfigureRequestLocalization(app, builder.Configuration);
        AddConfigurations(app);

        app.Run();
    }

    [Obsolete]
    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddRazorPages();
        services.AddControllers(options =>
         {
             options.Filters.Add<ExceptionFilter>();

         }).AddFluentValidation(options =>
         {
             options.RegisterValidatorsFromAssemblyContaining<RegistrationRequestValidator>();
             options.DisableDataAnnotationsValidation = true;
             options.LocalizationEnabled = true;
         });

        SwaggerConfiguration(services, configuration);

        ApplicationContextConfiguration(services, configuration);
        JwtTokenConfiguration(services, configuration);

        AddApplicationServices(services);
    }

    private static void ConfigureAppSettings(IHostBuilder hostBuilder, IHostEnvironment environment)
    {
        hostBuilder.ConfigureAppConfiguration(config =>
        {
            config
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
        });
    }

    private static void AddAppSettings(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtTokenSettings>(configuration.GetSection(nameof(JwtTokenSettings)));
        services.Configure<EmailSenderSettings>(configuration.GetSection(nameof(EmailSenderSettings)));
        services.Configure<RequestLocalizationSettings>(configuration.GetSection(nameof(RequestLocalizationSettings)));
        services.Configure<SwaggerSettings>(configuration.GetSection(nameof(SwaggerSettings)));

    }

    private static void ConfigureRequestLocalization(IApplicationBuilder app, IConfiguration configuration)
    {
        RequestLocalizationSettings requestLocalization = new();
        configuration.GetSection(nameof(RequestLocalizationSettings)).Bind(requestLocalization);

        IList<CultureInfo> supportedCultures = new List<CultureInfo>();
        requestLocalization.SupportedCultures.ToList()
            .ForEach(x => supportedCultures.Add(new CultureInfo(x)));

        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(requestLocalization.DefaultRequestCulture),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });
    }

    private static void AddConfigurations(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseMiddleware<TokenValidatorMiddleware>();
        app.UseBlazorFrameworkFiles();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.UseAntiforgery();
        app.MapControllers();
        app.MapFallbackToFile("index.html");
    }

    private static void ApplicationContextConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services
            .AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationContext>();
    }

    private static void JwtTokenConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        JwtTokenSettings jwtTokenSettings = new();
        configuration.GetSection(nameof(JwtTokenSettings)).Bind(jwtTokenSettings);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtTokenSettings.Issuer,
                ValidAudience = jwtTokenSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSettings.Key))
            };
        });
    }

    private static void SwaggerConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        SwaggerSettings swaggerSettings = new();
        configuration.GetSection(nameof(SwaggerSettings)).Bind(swaggerSettings);

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(swaggerSettings.Version, new OpenApiInfo
            {
                Title = swaggerSettings.Title,
                Version = swaggerSettings.Version,
            });

            //string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //options.IncludeXmlComments(xmlPath);

            options.AddSecurityDefinition(swaggerSettings.SecurityDefinitionType, new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = swaggerSettings.Description,
                Name = swaggerSettings.SecurityDefinitionName,
                Type = SecuritySchemeType.Http,
                BearerFormat = swaggerSettings.BearerFormat,
                Scheme = swaggerSettings.Scheme,
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type= ReferenceType.SecurityScheme,
                            Id= swaggerSettings.SecurityDefinitionType,
                        }
                    },
                    new string[]{}
                }
            });
        });

        services.AddFluentValidationRulesToSwagger();
    }

    private static void AddApplicationServices(IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IJwtTokenManager, JwtTokenManager>();
        services.AddScoped<IEmailSenderService, EmailSenderService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
    }
}