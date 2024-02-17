using Microsoft.OpenApi.Models;
using ModernRecrut.Documents.API.Interfaces;
using ModernRecrut.Documents.API.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFichierService,FichierService>();
builder.Logging.ClearProviders();

builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventLog();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API pour la gestion des documents",
        Version = "v1",
        Description = "API pour la gestion des documents d'un utilisateur",
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
