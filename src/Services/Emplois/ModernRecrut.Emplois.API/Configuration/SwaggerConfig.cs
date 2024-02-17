using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ModernRecrut.Emplois.API.Configuration
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Modern Recrut Emplois API",
                    Version = "v1",
                    Description = "API pour la gestion des offres d'emplois",
                    License = new OpenApiLicense
                    {
                        Name = "Apache 2.0",
                        Url = new Uri("http://www.apache.org")
                    },
                    Contact = new OpenApiContact
                    {
                        Name = "Yoshi",
                        Email = "yoshi@gmail.com",
                        Url = new Uri("https://cat-bounce.com/")
                    }
                });
                //Activation du support des commentaires XML dans Swagger UI
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
        public static void UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
