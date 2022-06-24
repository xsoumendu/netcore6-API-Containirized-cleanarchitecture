using ContainerApp.Contracts.Config;
using ContainerApp.Contracts.Services;
using ContainerApp.Core.Options;
using ContainerApp.Core.Services;
using ContainerApp.Core.Services.Jwt;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace ContainerApp.Core
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtTokenConfig>(configuration.GetSection("JwtTokenConfig"))
                .AddSingleton(x => x.GetRequiredService<IOptions<JwtTokenConfig>>().Value);

            services.AddLogging(options =>
            {
                options.AddConsole();
            });

            //services.AddMemoryCache();
            services.AddHttpContextAccessor();

            // register services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();

            // if DistributedCaching is enabled
            // return an instance of DistributedCachingService implementation
            // else return an In-Memory caching implementation
            services.AddSingleton<ICachingService>(x =>
            {
                if (configuration.GetValue<bool>("IsDistributedCachingEnabled"))
                {
                    return ActivatorUtilities.CreateInstance<DistributedCachingService>(x);
                }
                else
                {
                    return ActivatorUtilities.CreateInstance<CachingService>(x);
                }
            });

            //services.AddNCacheDistributedCache(configuration =>
            //{
            //    configuration.CacheName = "myCache";
            //    configuration.EnableLogs = true;
            //    configuration.ExceptionsEnabled = true;
            //});

            //services.AddNCacheDistributedCache(configuration.GetSection("NCacheSettings"));

            return services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddMediatR(Assembly.GetExecutingAssembly());
        }

        public static IServiceCollection AddSwaggerWithVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(1, 0);
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            services.ConfigureOptions<ConfigureSwaggerOptions>()
                .AddSwaggerGen(options =>
                {
                    // for further customization
                    // options.OperationFilter<DefaultValuesFilter>();
                });

            return services;
        }

        public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services)
        {
            services.ConfigureOptions<ConfigureJwtBearerOptions>()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            return services;
        }
    }
}
