using ModernRecrut.Emplois.Core.Interfaces;
using ModernRecrut.Emplois.Core.Services;
using ModernRecrut.Emplois.Infrastructure;

namespace ModernRecrut.Emplois.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));

            services.AddScoped<IEmploiService, EmploiService>();
        }
    }
}
