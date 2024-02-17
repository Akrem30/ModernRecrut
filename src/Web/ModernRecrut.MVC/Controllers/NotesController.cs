using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModernRecrut.MVC.Areas.Identity.Data;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.DTO;
using System.Security.Claims;

namespace ModernRecrut.MVC.Controllers
{
    [Authorize(Roles = "Admin, RH")]
    public class NotesController : Controller
    {
        private readonly INotesService _notesService;
        private readonly IPostulationsService _postulationsSerivce;
        private readonly IOffresEmploiService _emploisService;
		private readonly UserManager<Utilisateur> _gestionnaireUtilisateur;
		public NotesController(INotesService notesService, IPostulationsService postulationsSerivce,UserManager<Utilisateur> gestionnaireUtilisateur, IOffresEmploiService emploisService) 
        {
            _notesService = notesService;
            _postulationsSerivce = postulationsSerivce;
            _gestionnaireUtilisateur = gestionnaireUtilisateur;
            _emploisService = emploisService;
        }
        // GET: NotesController
        public async Task<ActionResult> Index()
        {
            var postulations = await _postulationsSerivce.ObtenirPostulations();

            
            if (postulations.Any())
            {
                for (int i = 0; i < postulations.Count; i++)
                {

                    postulations[i].OffreEmploi = await _emploisService.ObtenirOffreEmploiSelonId(postulations[i].OffreEmploiID);
                    postulations[i].Candidat = await _gestionnaireUtilisateur.FindByIdAsync(postulations[i].CandidatID.ToString());
                }
            }

            return View(postulations.Where(e => e.OffreEmploi != null));
        }
        public async Task<ActionResult> MesNotes()
        {
            var emetteurID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var emetteur = await _gestionnaireUtilisateur.FindByIdAsync(emetteurID);

            var nomEmeteur = emetteur.Nom + " " + emetteur.Prenom;
            var notes = await _notesService.ObtenirNotes();
            return View(notes.Where(n => n.NomEmetteur == nomEmeteur));
        }
        // GET: NotesController/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            var note = await _notesService.ObtenirNoteSelonId(id);
            return View(note);
        }

        // GET: NotesController/Create
        public async Task< ActionResult> Create(Guid id)
        {
			var emetteurID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var emetteur = await _gestionnaireUtilisateur.FindByIdAsync(emetteurID);
        
            var nomEmeteur = emetteur.Nom + " " + emetteur.Prenom;
			RequeteNote note = new RequeteNote { PostulationID = id, NomEmetteur = nomEmeteur};
            return View(note);
        }

        // POST: NotesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RequeteNote requeteNote)
        {
            

            if (ModelState.IsValid)
            {
                await _notesService.AjouterNote(requeteNote);
                return RedirectToAction(nameof(MesNotes));
            }
            return View();
        }

        // GET: NotesController/Edit/5
        public async Task<ActionResult> Edit(Guid id)
        {
            var note = await _notesService.ObtenirNoteSelonId(id);
            return View(note);
        }

        // POST: NotesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Note note)
        {
            if (ModelState.IsValid)
            {
                await _notesService.ModifierNote(note);
                return RedirectToAction(nameof(MesNotes));
            }
            return View(note);
        }

        // GET: NotesController/Delete/5
        public async Task<ActionResult> Delete(Guid id)
        {
            var note = await _notesService.ObtenirNoteSelonId(id);
            return View(note);
        }

        // POST: NotesController/Delete/5
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            
            await _notesService.SupprimerNote(id);
            return RedirectToAction(nameof(MesNotes));
        }
    }
}
