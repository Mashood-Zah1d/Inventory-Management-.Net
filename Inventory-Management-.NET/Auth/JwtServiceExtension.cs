using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Inventory_Management_.NET.Auth
{
    public static class JwtServiceExtension
    {
        public static IServiceCollection AddJwtAuth(this IServiceCollection services,
        IConfiguration config)
        {
            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                var key = Encoding.UTF8.GetBytes(config["Jwt:Key"]);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.ContainsKey("JwtToken"))
                        {
                            context.Token = context.Request.Cookies["JwtToken"];
                        }
                        return Task.CompletedTask;
                    }
                };
            });


            return services;
        }

    }
}
