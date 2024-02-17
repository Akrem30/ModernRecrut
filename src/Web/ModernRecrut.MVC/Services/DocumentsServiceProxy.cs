using Microsoft.AspNetCore.Mvc;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.DTO;

namespace ModernRecrut.MVC.Services
{
    public class DocumentsServiceProxy : IDocumentsService
    {
        private readonly string _documentsApiUrl = "/api/Documents/";
        private readonly HttpClient _httpClient;
        private readonly ILogger<DocumentsServiceProxy> _logger;
        private string? action;
        public DocumentsServiceProxy(HttpClient httpClient, ILogger<DocumentsServiceProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task EnregistrerDocument(RequeteFichier fichier)
        {
            HttpResponseMessage response;
            using (var contenu = new MultipartFormDataContent())
            using (var contenuFichier = new StreamContent(fichier.FileDetails.OpenReadStream()))
            {
                contenu.Add(contenuFichier, "fichier.FileDetails", fichier.FileDetails.FileName);

                contenu.Add(new StringContent(fichier.UtilisateurID.ToString()), "UtilisateurID");
                contenu.Add(new StringContent(fichier.TypeDocument.ToString()), "TypeDocument");

                response = await _httpClient.PostAsync(_documentsApiUrl, contenu);
            }

            action = "création";
            if (response.IsSuccessStatusCode)
                JournaliserCodeStatutSucces(action, fichier.UtilisateurID, null);

            if (!response.IsSuccessStatusCode)
                JournaliserCodeStatutSansSucces(response, action, fichier.UtilisateurID);
        }

        public async Task<List<string>> ObtenirDocumentsSelonId(Guid utilisateurID)
        {
            var reponse = await _httpClient.GetAsync(_documentsApiUrl + utilisateurID);
            action = "lecture";

            if (reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSucces(action, utilisateurID, null);

            if (!reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSansSucces(reponse, action, null);

            return await reponse.Content.ReadFromJsonAsync<List<string>>();
        }
        public async Task<string> LireFichier(string nomFichier)
        {
            var response = await _httpClient.GetAsync(_documentsApiUrl + "documents/" + nomFichier);
            string lienFichier = response.RequestMessage.RequestUri.ToString();

            action = "lecture";

            if (response.IsSuccessStatusCode)
                JournaliserCodeStatutSucces(action, null, nomFichier);

            if (!response.IsSuccessStatusCode)
                JournaliserCodeStatutSansSucces(response, action, null);

            return lienFichier;
        }
        public async Task SupprimerDocument(string nomFichier)
        {
            var response = await _httpClient.DeleteAsync(_documentsApiUrl + nomFichier);
            action = "suppression";

            if (response.IsSuccessStatusCode)
                JournaliserCodeStatutSucces(action, null, nomFichier);

            if (!response.IsSuccessStatusCode)
                JournaliserCodeStatutSansSucces(response, action, null);
        }
        private void JournaliserCodeStatutSansSucces(HttpResponseMessage codeStatut, string action, Guid? id)
        {
            int codeStatutNumerique = (int)codeStatut.StatusCode;

            if (codeStatutNumerique >= 400 && codeStatutNumerique <= 499)
            {
                if (id == Guid.Empty)
                    _logger.LogError(codeStatutNumerique, $"Erreur avec le code de statut : {codeStatutNumerique};  Survenu au cours de : {action}");
                else
                    _logger.LogError(codeStatutNumerique, $"Erreur avec le code de statut : {codeStatutNumerique};  Survenu au cours de : {action} du document ");
            }

            if (codeStatutNumerique >= 500 && codeStatutNumerique <= 599)
            {
                if (id == Guid.Empty)
                    _logger.LogCritical(codeStatutNumerique, $"Erreur avec le code de statut : {codeStatutNumerique};  Survenu au cours de : {action}");
                else
                    _logger.LogCritical(codeStatutNumerique, $"Erreur avec le code de statut : {codeStatutNumerique};  Survenu au cours de : {action} du document");
            }

        }

        private void JournaliserCodeStatutSucces(string action, Guid? id, string? nomFichier)
        {
            switch (action)
            {
                case "création":
                    _logger.LogInformation(CustomLogEventsDocuments.Creation, $"Ajout du document pour l'utilisateur {id}");
                    break;
                case "lecture":
                    _logger.LogInformation(CustomLogEventsDocuments.Lecture, $"Lecture du document");
                    break;
                case "suppression":
                    _logger.LogInformation(CustomLogEventsDocuments.Suppression, $"Suppression du document avec nom {nomFichier}");
                    break;
            }
        }
    }
}