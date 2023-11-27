using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApp.Client.Pages;
using WebApp.Client.Services.Implementations;
using WebApp.Client.Services.Interfaces;
using WebApp.Common.Configurations;
using WebApp.Components;
using WebApp.Data;
using WebApp.Data.Entities;
using WebApp.Filters;
using WebApp.Middleware;
using WebApp.Services.Implementations;
using WebApp.Services.Interfaces;

internal class Program
{
    [Obsolete]
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();
        AddConfigurations(app);

        app.Run();
    }

    [Obsolete]
    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<JwtTokenSettings>(configuration.GetSection(nameof(JwtTokenSettings)));

        services
            .Configure<EmailSenderSettings>(configuration.GetSection(nameof(EmailSenderSettings)));

        services
            .AddRazorComponents()
            .AddInteractiveWebAssemblyComponents();

        services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7061/") });


        services.AddControllers(options =>
         {
             options.Filters.Add<ExceptionFilter>();

         }).AddFluentValidation(options =>
         {
             //options.RegisterValidatorsFromAssemblyContaining<RegistrationRequestValidator>();
             options.DisableDataAnnotationsValidation = true;
             options.LocalizationEnabled = true;
         });

        SwaggerConfiguration(services);

        ApplicationContextConfiguration(services, configuration);
        JwtTokenConfiguration(services, configuration);

        AddApplicationServices(services);
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

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(Counter).Assembly);
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

    private static void SwaggerConfiguration(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.Configure<ApiBehaviorOptions>(options
            => options.SuppressModelStateInvalidFilter = true);
    }

    private static void AddApplicationServices(IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAccountClientService, AccountClientService>();
        services.AddScoped<IJwtTokenManager, JwtTokenManager>();
        services.AddScoped<IEmailSenderService, EmailSenderService>();
    }
}