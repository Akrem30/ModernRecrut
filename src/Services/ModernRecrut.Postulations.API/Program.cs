using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ModernRecrut.Postulations.API.Data;
using ModernRecrut.Postulations.API.Interfaces;
using ModernRecrut.Postulations.API.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<PostulationContext>(options =>
  options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection")));
builder.Services.AddScoped<IPostulationsService,PostulationsService>();
builder.Services.AddScoped<IGenerationEvaluationService,GenerationEvaluationService>();
builder.Services.AddScoped<INotesService,NotesService>();
builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));
builder.Services.AddHttpClient<IDocumentsService, DocumentsServiceProxy>(client =>
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("DocumentsUrlAPI")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API pour la gestion des postulations et des notes",
        Version = "v1",
        Description = "API pour la gestion des postulations des candidats",
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
