using ModernRecrut.MVC.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ModernRecrut.MVC.Data;
using ModernRecrut.MVC.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ModernRecrutMVCContextConnection") ?? throw new InvalidOperationException("Connection string 'ModernRecrutMVCContextConnection' not found.");

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlite(connectionString));

builder.Logging.AddLoggerConfiguration();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddIdentityConfiguration();
builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseForwardedHeaders();
app.UseStatusCodePagesWithRedirects("/Home/CodeStatus?code={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
