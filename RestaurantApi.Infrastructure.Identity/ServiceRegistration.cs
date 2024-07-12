using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RestaurantApi.Core.Application.Dtos.Account;
using RestaurantApi.Core.Application.Interfaces.Services;
using RestaurantApi.Core.Domain.Settings;
using RestaurantApi.Infrastructure.Identity.Contexts;
using RestaurantApi.Infrastructure.Identity.Entities;
using RestaurantApi.Infrastructure.Identity.Services;
using System.Text;

namespace RestaurantApi.Infrastructure.Identity;

public static class ServiceRegistration
{
    public static void AddIdentityLayer(this IServiceCollection services, IConfiguration configuration)
    {
        #region Contexts

        services.AddDbContext<IdentityContext>(options =>
        {
            options.EnableSensitiveDataLogging();
            options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
            m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
        });

        #endregion

        #region Identity
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/User";
            options.AccessDeniedPath = "/User/AccessDenied";
        });

        services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = configuration["JWTSettings:Issuer"],
                ValidAudience = configuration["JWTSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
            };
            options.Events = new JwtBearerEvents()
            {
                OnAuthenticationFailed = c =>
                {
                    c.NoResult();
                    c.Response.StatusCode = 500;
                    c.Response.ContentType = "text/plain";
                    return c.Response.WriteAsync(c.Exception.ToString());
                },
                OnChallenge = c =>
                {
                    c.HandleResponse();
                    c.Response.StatusCode = 401;
                    c.Response.ContentType = "application/json";
                    var result = JsonConvert.SerializeObject(new JwtResponse { HasError = true, Error = "You are not authorized, please authenticate." });
                    return c.Response.WriteAsync(result);
                },
                OnForbidden = c =>
                {
                    c.Response.StatusCode = 403;
                    c.Response.ContentType = "application/json";
                    var result = JsonConvert.SerializeObject(new JwtResponse { HasError = true, Error = "You are not allowed to access to this endpoint." });
                    return c.Response.WriteAsync(result);
                }
            };

        });
        #endregion

        #region Services
        services.AddTransient<IAccountService, AccountService>();
        #endregion

    }
}