using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.DTO;
namespace ModernRecrut.MVC.Interfaces
{
    public interface INotesService
    {
        public Task<List<Note>> ObtenirNotes();
        public Task<Note> ObtenirNoteSelonId(Guid id);
        public Task AjouterNote(RequeteNote requeteNote);
        public Task ModifierNote(Note note);
        public Task SupprimerNote(Guid id);
    }
}
