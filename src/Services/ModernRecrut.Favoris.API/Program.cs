using Microsoft.OpenApi.Models;
using ModernRecrut.Favoris.API.Interfaces;
using ModernRecrut.Favoris.API.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddMemoryCache(options => { options.SizeLimit = 5000000; });

builder.Services.AddHttpClient<IFavorisServiceProxy, FavorisServiceProxy>(client =>
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("UrlAPI")));

builder.Services.AddScoped<IFavorisService, FavorisService>();

builder.Services.AddResponseCaching();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API pour la gestion des favoris",
        Version = "v1",
        Description = "API pour la gestion des offres d'emplois favoris d'un utilisateur",
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
var app = builder.Build();
app.UseResponseCaching();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStatusCodePages();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
