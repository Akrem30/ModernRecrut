using ModernRecrut.Postulations.API.Interfaces;

namespace ModernRecrut.Postulations.API.Services
{
    public class DocumentsServiceProxy : IDocumentsService
    {
        private readonly string _documentsApiUrl = "/api/Documents/";
        private readonly HttpClient _httpClient;

        public DocumentsServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<string>> ObtenirDocumentsSelonId(Guid utilisateurID)
        {
            var reponse = await _httpClient.GetAsync(_documentsApiUrl + utilisateurID);

            return await reponse.Content.ReadFromJsonAsync<List<string>>();
        }
    }
}
