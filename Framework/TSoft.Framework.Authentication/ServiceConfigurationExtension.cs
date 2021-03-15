using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSoft.Framework.ApiUtils.Authentication;

namespace TSoft.Framework.Authentication
{
    public static class ServiceConfigurationExtension
    {
        public static IServiceCollection RegisterAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtAuthentication = configuration.GetSection(nameof(JWTAuthenticaion)).Get<JWTAuthenticaion>();

            byte[] key = System.Text.Encoding.ASCII.GetBytes(jwtAuthentication.Key);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(x =>
               {
                   x.RequireHttpsMetadata = false;
                   x.SaveToken = true;
                   x.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(key),
                       ValidateIssuer = false,
                       ValidateAudience = false
                   }; ;
                   x.Events = new JwtBearerEvents
                   {
                       OnMessageReceived = context =>
                       {
                           if (context.Request.Query.TryGetValue("JWTAuthenticaion", out StringValues token)
                           )
                           {
                               context.Token = token;
                           }

                           return Task.CompletedTask;
                       },
                       OnAuthenticationFailed = context =>
                       {
                           var te = context.Exception;
                           return Task.CompletedTask;
                       }
                   };
               });

            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("PermissionAuthorization", policy =>
                  policy.Requirements.Add(new PermissionAuthorizationRequirement()));
            });

            return services;
        }
    }
}
