using Microsoft.EntityFrameworkCore;
using ModernRecrut.Postulations.API.Data;
using ModernRecrut.Postulations.API.Entities;
using ModernRecrut.Postulations.API.Entities.DTO;
using ModernRecrut.Postulations.API.Interfaces;

namespace ModernRecrut.Postulations.API.Services
{
    public class NotesService : INotesService
    {
        private readonly PostulationContext _context;
        public NotesService(PostulationContext context)
        {
            _context = context;
        }
        public async Task AjouterNote(RequeteNote requeteNote)
        {
            Note note = new Note
            {
                
                Description = requeteNote.Description,
                NomEmetteur = requeteNote.NomEmetteur,
                PostulationID = requeteNote.PostulationID
            };
           await _context.AddAsync(note);
            await _context.SaveChangesAsync();
        }

        public async Task ModifierNote(Note note)
        {

            var noteAmodifier = await ObtenirNoteSelonId(note.Id);
            noteAmodifier.NomEmetteur = note.NomEmetteur;
            noteAmodifier.Description=note.Description;
           noteAmodifier.PostulationID = note.PostulationID;
             _context.Update(noteAmodifier);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Note>> ObtenirNotes()
        {
            return await _context.Notes.ToListAsync();
        }

        public async Task<Note> ObtenirNoteSelonId(Guid id)
        {
            return await _context.Notes.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task SupprimerNote(Guid id)
        {
            var noteAsupprimer = await ObtenirNoteSelonId(id);
             _context.Remove(noteAsupprimer);
            await _context.SaveChangesAsync();
        }
    }
}
