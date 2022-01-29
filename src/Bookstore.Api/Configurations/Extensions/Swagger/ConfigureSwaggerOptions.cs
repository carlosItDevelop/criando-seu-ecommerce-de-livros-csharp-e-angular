using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace APIBookstore.Api.Configurations.Extensions.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "API Cooperchip - Digital Innovation One",
                Version = description.ApiVersion.ToString(),
                Description = "Esta API faz parte do curso Criando seu e-commerce de livros em C# e Angular.",
                Contact = new OpenApiContact()
                {
                    Name = "Carlos A Santos",
                    Email = "contato.cooperchip@gmail.com",
                    Url = new Uri("https://cooperchip.com.br")
                },
                TermsOfService = new Uri("https://opensource.org/licenses/MIT"),
                License = new OpenApiLicense()
                {
                    Name = "MIT",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            };

            if (description.IsDeprecated)
            {
                info.Description += " *** ESTA VERSÃO ESTÁ OBSOLETA ***";
            }

            return info;
        }
    }



}
