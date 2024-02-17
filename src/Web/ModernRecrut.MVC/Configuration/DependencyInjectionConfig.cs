using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Services;

namespace ModernRecrut.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IOffresEmploiService, OffresEmploiServiceProxy>(client => 
                    client.BaseAddress = new Uri(configuration.GetValue<string>("OffresEmploiUrlAPI")));

            services.AddHttpClient<IFavorisService, FavorisServiceProxy>(client =>
                    client.BaseAddress = new Uri(configuration.GetValue<string>("FavorisUrlAPI")));

            services.AddHttpClient<IDocumentsService, DocumentsServiceProxy>(client =>
                    client.BaseAddress = new Uri(configuration.GetValue<string>("DocumentsUrlAPI")));

            services.AddHttpClient<IPostulationsService, PostulationsServiceProxy>(client =>
                   client.BaseAddress = new Uri(configuration.GetValue<string>("PostulationsUrlAPI")));
           
            services.AddHttpClient<INotesService, NotesServiceProxy>(client =>
                   client.BaseAddress = new Uri(configuration.GetValue<string>("PostulationsUrlAPI")));

        }
    }
}
