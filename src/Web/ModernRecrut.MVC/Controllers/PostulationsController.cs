using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModernRecrut.MVC.Areas.Identity.Data;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.DTO;
using System.Security.Claims;

namespace ModernRecrut.MVC.Controllers
{
    [Authorize]
    public class PostulationsController : Controller
    {
        private readonly IPostulationsService _postulationService;
        private readonly IDocumentsService _documentsService;
        private readonly IOffresEmploiService _emploisService;
        private readonly UserManager<Utilisateur> _userManager;
        private readonly INotesService _notesService;

        public PostulationsController(IPostulationsService postulationService, IDocumentsService documentsService, IOffresEmploiService offresEmploiService, UserManager<Utilisateur> userManager, INotesService notesService)
        {
            _postulationService = postulationService;
            _documentsService = documentsService;
            _emploisService = offresEmploiService;
            _userManager = userManager;
            _notesService = notesService;

        }

        [Authorize(Roles = "Admin, Candidat")]
        public async Task<IActionResult> Index()
        {
            var candidatID = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var postulations = await _postulationService.ObtenirPostulations();
            var postulationsCandidat = postulations.Where(p => p.CandidatID == candidatID).ToList();

            if (postulationsCandidat.Any())
            {
                for (int i = 0; i < postulationsCandidat.Count; i++)
                {

                    postulationsCandidat[i].OffreEmploi = await _emploisService.ObtenirOffreEmploiSelonId(postulationsCandidat[i].OffreEmploiID);
                    postulationsCandidat[i].Candidat = await _userManager.FindByIdAsync(postulationsCandidat[i].CandidatID.ToString());
                }
            }

            return View(postulationsCandidat);
        }

        [Authorize(Roles = "Admin, Candidat,RH")]
        public async Task<ActionResult> Details(Guid id)
        {
            var postulation = await _postulationService.ObtenirPostulationSelonId(id);
            postulation.OffreEmploi = await _emploisService.ObtenirOffreEmploiSelonId(postulation.OffreEmploiID);
            postulation.Candidat = await _userManager.FindByIdAsync(postulation.CandidatID.ToString());
            var notes = await _notesService.ObtenirNotes();
            postulation.Notes = notes.Where(e => e.PostulationID == postulation.Id).ToList();

            return View(postulation);
        }

        [Authorize(Roles = "Admin, Candidat")]
        public ActionResult Create(Guid id)
        {
            RequetePostulation postulation = new RequetePostulation { OffreEmploiID = id, CandidatID = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), DateDisponibilite = DateTime.Now.Date };
            return View(postulation);
        }

        [Authorize(Roles = "Admin, Candidat")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RequetePostulation requetePostulation)
        {
            var documentsCandidat = await _documentsService.ObtenirDocumentsSelonId(requetePostulation.CandidatID);
            if (!documentsCandidat.Any(d => d.Contains("CV")))
                ModelState.AddModelError(string.Empty, "Un CV est obligatoire pour postuler. Veuillez déposer au préalable un CV dans votre espace Documents");

            if (!documentsCandidat.Any(d => d.Contains("LETTREDEMOTIVATION")))
                ModelState.AddModelError(string.Empty, "Une lettre de motivation est obligatoire pour postuler. Veuillez déposer au préalable une lettre de motivation dans votre espace Documents");

            var postulations = await _postulationService.ObtenirPostulations();
            if (postulations.Where(p => p.CandidatID == requetePostulation.CandidatID).Any(p => p.OffreEmploiID == requetePostulation.OffreEmploiID))
                ModelState.AddModelError(string.Empty, "Vous avez déjà postulé à cette offre d'emploi");

            if (requetePostulation.PretentionSalariale > 150000)
                ModelState.AddModelError("PretentionSalariale", "Votre prétention salariale est au-delà de nos limites");

            if (requetePostulation.PretentionSalariale <= 0)
                ModelState.AddModelError("PretentionSalariale", "Veuillez saisir une valeur valide pour le salaire");

            DateTime dateLimite = DateTime.Now.AddDays(45);
            if (requetePostulation.DateDisponibilite <= DateTime.Now || requetePostulation.DateDisponibilite > DateTime.Now.AddDays(45))
                ModelState.AddModelError("DateDisponibilite", $"La date de disponibilité doit être supérieure à la date du jour et inférieure à {dateLimite.ToString("yyyy-MM-dd")}");

            if (ModelState.IsValid)
            {
                await _postulationService.AjouterPostulation(requetePostulation);
                return RedirectToAction(nameof(Index));
            }
            return View(requetePostulation);

        }

        [Authorize(Roles = "Admin, Candidat")]
        public async Task<ActionResult> Edit(Guid id)
        {
            var postulation = await _postulationService.ObtenirPostulationSelonId(id);
            if (postulation == null)
                return NotFound();

            return View(postulation);
        }

        [Authorize(Roles = "Admin, Candidat")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Postulation postulation)
        {
            var postulationAmodifier = await _postulationService.ObtenirPostulationSelonId(postulation.Id);
            if (postulationAmodifier.DateDisponibilite > DateTime.Now.AddDays(5) || postulationAmodifier.DateDisponibilite < DateTime.Now.AddDays(-5))
            {
                ModelState.AddModelError("DateDisponibilite", "Vous ne pouvez pas modifier la date de disponibilité pour cette postulation");
                return View(postulation);
            }

            if (postulation.PretentionSalariale > 150000)
                ModelState.AddModelError("PretentionSalariale", "Votre prétention salariale est au-delà de nos limites");

            if (postulation.PretentionSalariale <= 0)
                ModelState.AddModelError("PretentionSalariale", "Veuillez saisir une valeur valide pour le salaire");

            DateTime dateLimite = DateTime.Now.AddDays(45);
            if (postulation.DateDisponibilite <= DateTime.Now || postulation.DateDisponibilite > DateTime.Now.AddDays(45))
                ModelState.AddModelError("DateDisponibilite", $"La date de disponibilité doit être supérieure à la date du jour et inférieure à {dateLimite.ToString("yyyy-MM-dd")}");

            if (ModelState.IsValid)
            {
                await _postulationService.ModifierPostulation(postulation);
                return RedirectToAction(nameof(Index));
            }
            return View(postulation);
        }

        [Authorize(Roles = "Admin, Candidat")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var postulation = await _postulationService.ObtenirPostulationSelonId(id);
            return View(postulation);
        }

        [Authorize(Roles = "Admin, Candidat")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var postulationAsupprimer = await _postulationService.ObtenirPostulationSelonId(id);
            if (postulationAsupprimer.DateDisponibilite > DateTime.Now.AddDays(5) || postulationAsupprimer.DateDisponibilite < DateTime.Now.AddDays(-5))
                ModelState.AddModelError("DateDisponibilite", "Vous ne pouvez pas supprimer cette postulation");

            await _postulationService.SupprimerPostulation(id);
            return RedirectToAction(nameof(Index));
        }
    }
}