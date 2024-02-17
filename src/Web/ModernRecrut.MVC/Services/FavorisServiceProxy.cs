using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace ModernRecrut.MVC.Services
{
    public class FavorisServiceProxy : IFavorisService
    {
        private readonly HttpClient _httpClient;
        private const string _favorisApiUrl = "api/Favoris/";
        private readonly ILogger<FavorisServiceProxy> _logger;
        private string? action;

        public FavorisServiceProxy(HttpClient httpClient, ILogger<FavorisServiceProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task AjouterFavorite(RequeteAjoutFavorite requeteAjoutFavorite)
        {
            var reponse = await _httpClient.PostAsJsonAsync(_favorisApiUrl, requeteAjoutFavorite);
            action = "création";

            if (reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSucces(action, requeteAjoutFavorite.Cle, requeteAjoutFavorite.OffreEmploiID);

            if (!reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSansSucces(reponse, action, requeteAjoutFavorite.Cle, requeteAjoutFavorite.OffreEmploiID);      
        }

        public async Task<Favoris> ObtenirTout(string ipAddress)
        {
            var reponse = await _httpClient.GetAsync(_favorisApiUrl + ipAddress);
            action = "obtention de tous les favoris";

            if (!reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSansSucces(reponse, action, ipAddress, Guid.Empty);
 
            return await reponse.Content.ReadFromJsonAsync<Favoris>();
        }

        public async Task SupprimerFavorite(string ipAddress, Guid offreId)
        {
            var reponse = await _httpClient.DeleteAsync(_favorisApiUrl + $"{ipAddress}/{offreId}");
            action = "suppression";

            if (reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSucces(action, ipAddress, offreId);

            if (!reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSansSucces(reponse, action, ipAddress, offreId); 
        }

        private void JournaliserCodeStatutSansSucces(HttpResponseMessage codeStatut, string action, string cle, Guid id)
        {
            int codeStatutNumerique = (int)codeStatut.StatusCode;

            if (codeStatutNumerique >= 400 && codeStatutNumerique <= 499)
            {
                if (id == Guid.Empty)
                    _logger.LogError(codeStatutNumerique, $"Erreur avec le code de statut : {codeStatutNumerique}; Addresse IP : {cle}; Survenu au cours de : {action}");
                else
                    _logger.LogError(codeStatutNumerique, $"Erreur avec le code de statut : {codeStatutNumerique};  Addresse IP : {cle}; Survenu au cours de : {action} de l'offre d'emploi favorite avec l'identifiant {id}");
            }

            if (codeStatutNumerique >= 500 && codeStatutNumerique <= 599)
            {
                if (id == Guid.Empty)
                    _logger.LogCritical(codeStatutNumerique, $"Erreur avec le code de statut : {codeStatutNumerique}; Addresse IP : {cle}; Survenu au cours de : {action}");
                else
                    _logger.LogCritical(codeStatutNumerique, $"Erreur avec le code de statut : {codeStatutNumerique};  Addresse IP : {cle}; Survenu au cours de : {action} de l'offre d'emploi favorite avec l'identifiant {id}");
            }

        }

        private void JournaliserCodeStatutSucces(string action, string cle, Guid offreEmploiId)
        {
            switch (action)
            {
                case "création":
                    _logger.LogInformation(CustomLogEventsFavoris.Creation, $"Ajout du favori avec adresse ip de l'utilisateur {cle} et Id de l'offre {offreEmploiId}");
                    break;
                case "suppression":
                    _logger.LogInformation(CustomLogEventsFavoris.Suppression, $"Suppression du favori avec adresse ip de l'utilisateur {cle} et Id de l'offre {offreEmploiId}");
                    break;
            }
        }
    }
}
