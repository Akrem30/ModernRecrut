using Microsoft.Extensions.Logging;
using ModernRecrut.MVC.Controllers;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.DTO;

namespace ModernRecrut.MVC.Services
{
    public class OffresEmploiServiceProxy : IOffresEmploiService
    {
        private readonly HttpClient _httpClient;
        private const string _emploisApiUrl = "api/Emplois/";
        private readonly ILogger<OffresEmploiServiceProxy> _logger;
        private string? action;

        public OffresEmploiServiceProxy(HttpClient httpClient, ILogger<OffresEmploiServiceProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task AjouterOffreEmploi(RequeteAjoutOffreEmploi requeteAjoutOffreEmploi)
        {
            var reponse = await _httpClient.PostAsJsonAsync(_emploisApiUrl, requeteAjoutOffreEmploi);
            action = "création";

            if (reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSucces(action, Guid.Empty);

            if (!reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSansSucces(reponse, action, Guid.Empty);
        }

        public async Task<OffreEmploi> ObtenirOffreEmploiSelonId(Guid id)
        {
            var reponse = await _httpClient.GetAsync(_emploisApiUrl + id);
            action = "obtention d'offre d'emploi selon identifiant";

            if (!reponse.IsSuccessStatusCode) 
                JournaliserCodeStatutSansSucces(reponse, action, id);

            return await reponse.Content.ReadFromJsonAsync<OffreEmploi>();
        }

        public async Task<IEnumerable<OffreEmploi>> ObtenirTousOffresEmploi()
        {
            var reponse = await _httpClient.GetAsync(_emploisApiUrl);
            action = "obtention de tous les offres d'emploi";

            if (!reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSansSucces(reponse, action, Guid.Empty);

            return await reponse.Content.ReadFromJsonAsync<IEnumerable<OffreEmploi>>();
        }

        public async Task<IEnumerable<OffreEmploi>> ObtenirOffresEmploiValides()
        {
            var reponse = await _httpClient.GetAsync(_emploisApiUrl + "EmploisValide");
            action = "obtention d'offres d'emploi valides";

            if (!reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSansSucces(reponse, action, Guid.Empty);

            return await reponse.Content.ReadFromJsonAsync<IEnumerable<OffreEmploi>>();
        }

        public async Task ModifierOffreEmploi(OffreEmploi offreEmploi)
        {
            var reponse = await _httpClient.PutAsJsonAsync(_emploisApiUrl + offreEmploi.Id, offreEmploi);
            action = "modification";

            if (reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSucces(action, offreEmploi.Id);

            if (!reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSansSucces(reponse, action, offreEmploi.Id);
        }

        public async Task SupprimerOffreEmploi(Guid id)
        {
            var reponse = await _httpClient.DeleteAsync(_emploisApiUrl + id);
            action = "suppression";

            if (reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSucces(action, id);

            if (!reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSansSucces(reponse, action, id);
        }

        private void JournaliserCodeStatutSansSucces(HttpResponseMessage codeStatut, string action, Guid id)
        {
            int codeStatutNumerique = (int)codeStatut.StatusCode;

            if (codeStatutNumerique >= 400 && codeStatutNumerique <= 499)
            {
                if (id == Guid.Empty)
                    _logger.LogError(codeStatutNumerique, $"Erreur avec le code de statut : {codeStatutNumerique};  Survenu au cours de : {action}");
                else
                    _logger.LogError(codeStatutNumerique, $"Erreur avec le code de statut : {codeStatutNumerique};  Survenu au cours de : {action} de l'offre d'emploi avec l'identifiant {id}");
            }
                
            if (codeStatutNumerique >= 500 && codeStatutNumerique <= 599)
            {
                if (id == Guid.Empty)
                    _logger.LogCritical(codeStatutNumerique, $"Erreur avec le code de statut : {codeStatutNumerique};  Survenu au cours de : {action}");
                else
                    _logger.LogCritical(codeStatutNumerique, $"Erreur avec le code de statut : {codeStatutNumerique};  Survenu au cours de : {action} de l'offre d'emploi avec l'identifiant {id}");
            }
                
        }

        private void JournaliserCodeStatutSucces(string action, Guid id)
        {
            switch (action)
            {
                case "création":
                    _logger.LogInformation(CustomLogEventsOffresEmploi.Creation, $"Création d'une offre d'emploi à {DateTime.Now}");
                    break;
                case "modification":
                    _logger.LogInformation(CustomLogEventsOffresEmploi.Modification, $"Modification de l'offre d'emploi avec ID {id}");
                    break;
                case "suppression":
                    _logger.LogInformation(CustomLogEventsOffresEmploi.Suppression, $"Suppression de l'offre d'emploi avec ID {id}");
                    break;
            }
        }
    }
}
