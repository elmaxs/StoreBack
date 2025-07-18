using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Store.Infrastructure;
using System.Security.Cryptography;
using System.Text;

namespace Store.API.Extensions
{
    public static class ApiExtensions
    {
        public static void AddApiAuthentication(this IServiceCollection services, IOptions<JwtOptions> jwtOptions)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = false,
               ValidateAudience = false,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey))
           };

           options.Events = new JwtBearerEvents
           {
               OnMessageReceived = context =>
               {
                   // Підтримка і Authorization header, і куки
                   var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
                   if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                   {
                       context.Token = authHeader["Bearer ".Length..];
                   }
                   else
                   {
                       context.Token = context.Request.Cookies["testy-cookies"];
                   }

                   return Task.CompletedTask;
               }
           };
        });

            services.AddAuthorization();
        }
    }
}
