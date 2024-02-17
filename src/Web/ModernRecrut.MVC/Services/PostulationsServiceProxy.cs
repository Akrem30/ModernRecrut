using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.DTO;
using System;

namespace ModernRecrut.MVC.Services
{
    public class PostulationsServiceProxy : IPostulationsService
    {
        private readonly HttpClient _httpClient;
        private const string _postulationsApiUrl = "api/Postulations/";
        private readonly ILogger<PostulationsServiceProxy> _logger;
        private string? action;

        public PostulationsServiceProxy(HttpClient httpClient, ILogger<PostulationsServiceProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Postulation>> ObtenirPostulations()
        {
            var reponse = await _httpClient.GetAsync(_postulationsApiUrl);
            action = "lecture de la liste";

            if (reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSucces(action, Guid.Empty);

            if (!reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSansSucces(reponse, action, Guid.Empty);

            return await reponse.Content.ReadFromJsonAsync<List<Postulation>>();
        }

        public async Task<Postulation> ObtenirPostulationSelonId(Guid id)
        {
            var reponse = await _httpClient.GetAsync(_postulationsApiUrl + id);
            action = "lecture";

            if (reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSucces(action, id);

            if (!reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSansSucces(reponse, action, id);

            return await reponse.Content.ReadFromJsonAsync<Postulation>();
        }

        public async Task AjouterPostulation(RequetePostulation requetePostulation)
        {
            var reponse = await _httpClient.PostAsJsonAsync(_postulationsApiUrl, requetePostulation);
            action = "création";

            if (reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSucces(action, Guid.Empty);

            if (!reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSansSucces(reponse, action, Guid.Empty);
        }

        public async Task ModifierPostulation(Postulation postulation)
        {
            var reponse = await _httpClient.PutAsJsonAsync(_postulationsApiUrl + postulation.Id, postulation);
            action = "modification";

            if (reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSucces(action, postulation.Id);

            if (!reponse.IsSuccessStatusCode)
                JournaliserCodeStatutSansSucces(reponse, action, postulation.Id);
        }

        public async Task SupprimerPostulation(Guid id)
        {
            var reponse = await _httpClient.DeleteAsync(_postulationsApiUrl + id);
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
                    _logger.LogError(codeStatutNumerique, $"Erreur avec le code de statut : {codeStatutNumerique};  Survenu au cours de : {action} de la postulation avec l'identifiant {id}");
            }

            if (codeStatutNumerique >= 500 && codeStatutNumerique <= 599)
            {
                if (id == Guid.Empty)
                    _logger.LogCritical(codeStatutNumerique, $"Erreur avec le code de statut : {codeStatutNumerique};  Survenu au cours de : {action}");
                else
                    _logger.LogCritical(codeStatutNumerique, $"Erreur avec le code de statut : {codeStatutNumerique};  Survenu au cours de : {action} de la postulation avec l'identifiant {id}");
            }

        }

        private void JournaliserCodeStatutSucces(string action, Guid id)
        {
            switch (action)
            {
                case "lecture de la liste":
                    _logger.LogInformation(CustomLogEventsPostulations.Lecture, $"Lecture de l'ensemble des postulations");
                    break;
                case "lecture":
                    _logger.LogInformation(CustomLogEventsPostulations.Lecture, $"Lecture de la postulation avec ID {id}");
                    break;
                case "création":
                    _logger.LogInformation(CustomLogEventsPostulations.Creation, $"Création d'une postulation à {DateTime.Now}");
                    break;
                case "modification":
                    _logger.LogInformation(CustomLogEventsPostulations.Modification, $"Modification de la postulation avec ID {id}");
                    break;
                case "suppression":
                    _logger.LogInformation(CustomLogEventsPostulations.Suppression, $"Suppression de la postulation avec ID {id}");
                    break;
            }
        }
    }
}
