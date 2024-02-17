using ModernRecrut.Postulations.API.Entities;
using ModernRecrut.Postulations.API.Entities.DTO;

namespace ModernRecrut.Postulations.API.Interfaces
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
