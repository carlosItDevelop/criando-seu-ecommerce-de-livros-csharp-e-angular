using APIBookstore.Api.Configurations.AutoMapper;
using APIBookstore.Api.Configurations.Extensions;
using APIBookstore.Api.Configurations.Extensions.Swagger;
using Bookstore.Domain.Abstractions.Repository;
using Bookstore.Infra.Data.Orm;
using Bookstore.Infra.Repository.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace APIBookstore.Api
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
            // Duas formas de Registrar, estando no mesmo projeto da Startup Class
            //services.AddAutoMapper(typeof(Startup), typeof(AutoMapperConfig));
            services.AddAutoMapper(typeof(AutoMapperConfig));

            services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddCors(options =>
            {
                options.AddPolicy("Development", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                options.AddPolicy("Production", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

                //options.AddPolicy("Production", builder => builder.WithMethods("GET").WithOrigins("https://cooperchip.com.br")
                //            .SetIsOriginAllowedToAllowWildcardSubdomains().AllowAnyHeader());

            });

            #region: My Extensions Methods
            services.AddApiConfig();
            services.AddSwaggerConfig();
            services.AddScoped<IRepositoryProducts, RepositoryProducts>();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            #endregion

            services.AddControllers();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("Development");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseCors("Development"); // Usar apenas nas demos => Configuração Ideal: Production
                app.UseHsts();
            }
            app.UseSwaggerConfig(provider);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
