using Microsoft.AspNetCore.Mvc;
using ModernRecrut.Favoris.API.Interfaces;
using ModernRecrut.Favoris.API.Models;
using ModernRecrut.Favoris.API.Models.DTO;

namespace ModernRecrut.Favoris.API.Services
{
    public class FavorisServiceProxy : IFavorisServiceProxy
    {
        private readonly HttpClient _httpClient;
        private const string _offreEmploiApiUrl = "api/Emplois/";

        public FavorisServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<OffreEmploi>> ObtenirOffresEmploisValides()
        {
            var response = await _httpClient.GetAsync(_offreEmploiApiUrl + "EmploisValide");

            if (response.IsSuccessStatusCode)
            {
                var offreDetails = await response.Content.ReadFromJsonAsync<List<OffreEmploi>>();
                return offreDetails;
            }
            else
            {
                throw new BadHttpRequestException("Erreur lors de la récupération des détails de l'offre");
            }
        }
    }
}
