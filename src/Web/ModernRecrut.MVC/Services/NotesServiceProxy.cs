using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.DTO;

namespace ModernRecrut.MVC.Services
{
    public class NotesServiceProxy :INotesService
    {
        private readonly HttpClient _httpClient;
        private const string _notesApiUrl = "api/Notes/";
        public NotesServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Note>> ObtenirNotes()
        {
            var reponse = await _httpClient.GetAsync(_notesApiUrl);
            return await reponse.Content.ReadFromJsonAsync<List<Note>>();
        }

        public async Task<Note> ObtenirNoteSelonId(Guid id)
        {
            var reponse = await _httpClient.GetAsync(_notesApiUrl + id);


            return await reponse.Content.ReadFromJsonAsync<Note>();
        }

        public async Task AjouterNote(RequeteNote requeteNote)
        {
            var reponse = await _httpClient.PostAsJsonAsync(_notesApiUrl, requeteNote);
        }

        public async Task ModifierNote(Note note)
        {
            var reponse = await _httpClient.PutAsJsonAsync(_notesApiUrl + note.Id, note);
        }

        public async Task SupprimerNote(Guid id)
        {
            var reponse = await _httpClient.DeleteAsync(_notesApiUrl + id);
        }
    }
}
