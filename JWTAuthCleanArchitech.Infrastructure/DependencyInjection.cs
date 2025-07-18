using JWTAuthCleanArchitech.Infrastructure.Data;
using JWTAuthCleanArchitech.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthCleanArchitech.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      {
          options.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuer = true,
              ValidIssuer = configuration["AppSettings:Issuer"],
              ValidateAudience = true,
              ValidAudience = configuration["AppSettings:Audience"],
              ValidateLifetime = true,
           
              IssuerSigningKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(configuration["AppSettings:Token"]!)),
              ValidateIssuerSigningKey = true,
             RoleClaimType = ClaimTypes.Role,
              NameClaimType = ClaimTypes.Name
          };
      });
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IbookService, BookService>();
         
        
   
            return services;

        }

    }
}
