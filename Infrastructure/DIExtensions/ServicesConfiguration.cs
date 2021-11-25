using Core.Configs;
using Core.Interfaces.Services;
using Infrastructure.Services; 
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DIExtensions
{
    public static class ServicesConfiguration
    {
        public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            
            // Configuration
            services.AddMemoryCache();
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));          
            services.AddSingleton(configuration.GetSection("AppSettings").Get<AppSettings>()); 
           
            // Services 
            services.AddHttpClient();
            services.AddTransient<IHttpService, HttpService>(); 
            services.AddScoped<IServiceCompareFaces, ServiceCompareFaces>();
            services.AddScoped<IImageServiceUtils, ImageServiceUtils>();
            services.AddScoped<IServiceDetectFaces, ServiceDetectFaces>();
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            // Repository  
        }
        public static void AddDocumentationServices(this IServiceCollection services, string swaggerTitle = "")
        {
            services.AddSwaggerGen(c =>
            {
                string title = !string.IsNullOrEmpty(swaggerTitle) ? swaggerTitle : "DevPath";
                c.SwaggerDoc("v1", new OpenApiInfo { Title = $"{ swaggerTitle}", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            });
        }
    }
}
