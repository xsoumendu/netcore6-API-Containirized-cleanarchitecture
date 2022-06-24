using ContainerApp.Core;
using ContainerApp.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ContainerApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddPersistence(Configuration);
            services.AddCore(Configuration);

            // prevents Mvc to throw 400 on invalid RequestBody
            // this is since we're using Fluent to do the same
            // within the Action Method
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddJwtBearerAuthentication();

            //services.AddResponseCaching();

            services.AddControllers();

            services.AddSwaggerWithVersioning();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowAnyMethod();
            });

            app.UseSwaggerWithVersioning(provider);

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            //app.UseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
