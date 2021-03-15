using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Tsoft.Framework.CacheMemory;
using Tsoft.Framework.Common.Configs;

namespace TSoft.Framework.ApiUtils
{
    public static class ServiceConfigurationExtensions
    {
        public static IServiceCollection RegisterCommonServiceComponents(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterCacheMemory(configuration);
            services.RegisterSwaggerServiceComponents(configuration, Assembly.GetExecutingAssembly());

            services.Configure<FormOptions>(options =>
            {
                // options.ValueCountLimit = 200; // 200 items max
                options.ValueLengthLimit = 1024 * 1024 * 100; // 100MB max len form data
            });

            services.AddCors();
            services.AddMvc();
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });

            return services;
        }

        public static ILoggerFactory RegisterLoggingComponents(this ILoggerFactory loggerFactory)
        {
            loggerFactory.CreateLogger("Logs/Tsoft-{Date}.txt");
            return loggerFactory;
        }

        public static IServiceCollection RegisterSwaggerServiceComponents(this IServiceCollection services, IConfiguration configuration, Assembly currentAssembly)
        {
            var project = configuration.GetSection(nameof(Project)).Get<Project>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(project.Version, new OpenApiInfo { Title = project.Name, Version = project.Version });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Example: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VybmFtZSI6ImhpZXVuZCIsImp0aSI6IjgyMzliNDc5LWY5OTgtNDk0OC1hZGUyLTdlMjhmYTJjYTc4YyIsInN1YiI6IjlhMDYzYmU0LTY5MzktNDhhZi1hOTIyLTA4ZDg5Zjc4OGQ0MCIsImV4cCI6MTYwNzkxNjgyNywiaXNzIjoiVHNvZnQiLCJhdWQiOiJUc29mdCJ9.Vzdm3wjD_kfIyfap8ZPsaPlQvOFolcA2KgmhgvJOWS0'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                            {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
                // Set the comments path for the Swagger JSON and UI.
                StackFrame[] frames = new StackTrace().GetFrames();
                var initialAssembly = Assembly.GetEntryAssembly();

                var xmlFile = $"{initialAssembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });

            return services;
        }

        public static IApplicationBuilder UseCommonConfig(this IApplicationBuilder app)
        {
            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseHttpsRedirection();
            return app;
        }

        public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder applicationBuilder, IConfiguration configuration)
        {
            applicationBuilder.UseSwagger();
            applicationBuilder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            return applicationBuilder;
        }
    }
}
