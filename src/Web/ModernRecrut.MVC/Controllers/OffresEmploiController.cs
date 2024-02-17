using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.DTO;

namespace ModernRecrut.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OffresEmploiController : Controller
    {
        private readonly IOffresEmploiService _offresEmploiService;

        public OffresEmploiController(IOffresEmploiService offresEmploiService)
        {
            _offresEmploiService = offresEmploiService;
        }

        // GET: EmploisController
        [AllowAnonymous]
        public async Task<ActionResult> Index(string saisieRecherche)
        {
            ViewData["FiltreActuel"] = saisieRecherche;

            var offresEmploi = await _offresEmploiService.ObtenirOffresEmploiValides();

            if (!String.IsNullOrEmpty(saisieRecherche))
                offresEmploi = offresEmploi.Where(e => e.Poste.Contains(saisieRecherche, StringComparison.InvariantCultureIgnoreCase));

            return View(offresEmploi);
        }

        // GET: EmploisController/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(Guid id)
        {
            var offreEmploi = await _offresEmploiService.ObtenirOffreEmploiSelonId(id);
            return View(offreEmploi);
        }

        // GET: EmploisController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmploisController/Create
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RequeteAjoutOffreEmploi requeteAjoutOffreEmploi)
        {
            if (requeteAjoutOffreEmploi.DateAffichage < DateTime.Now.Date)
                ModelState.AddModelError(string.Empty, "La date d'affichage doit être égale ou supérieure à la date du jour.");

            if (requeteAjoutOffreEmploi.DateFin <= DateTime.Now.Date || requeteAjoutOffreEmploi.DateFin <= requeteAjoutOffreEmploi.DateAffichage)
                ModelState.AddModelError(string.Empty, "La date de fin doit être supérieure à la date du jour et à la date d'affichage.");

            if (ModelState.IsValid)
            {
                await _offresEmploiService.AjouterOffreEmploi(requeteAjoutOffreEmploi);
                return RedirectToAction(nameof(Index));
            }

            return View(requeteAjoutOffreEmploi);
        }

        // GET: EmploisController/Edit/5
      
        public async Task<ActionResult> Edit(Guid id)
        {
            return View(await _offresEmploiService.ObtenirOffreEmploiSelonId(id));
        }

        // POST: EmploisController/Edit/5
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(OffreEmploi offreEmploi)
        {
            var offreEmploiVerification = await _offresEmploiService.ObtenirOffreEmploiSelonId(offreEmploi.Id);

            if (offreEmploiVerification.DateFin < DateTime.Now.Date)
                ModelState.AddModelError(string.Empty, "Cette offre d'emploi n'est plus modifiable");

            if (offreEmploi.DateAffichage != offreEmploiVerification.DateAffichage)
                ModelState.AddModelError(string.Empty, "La date d'affichage ne peut pas être modifiée ");

            if (offreEmploi.DateFin <= DateTime.Now.Date || offreEmploi.DateFin <= offreEmploi.DateAffichage)
                ModelState.AddModelError(string.Empty, "La date de fin doit être supérieure à la date du jour et à la date d'affichage.");

            if (ModelState.IsValid)
            {
                await _offresEmploiService.ModifierOffreEmploi(offreEmploi);
                return RedirectToAction(nameof(Index));
            }

            return View(offreEmploi);
        }

        // GET: EmploisController/Delete/5
        
        public async Task<ActionResult> Delete(Guid id)
        {
            var offreEmploi = await _offresEmploiService.ObtenirOffreEmploiSelonId(id);
            return View(offreEmploi);
        }

        // POST: EmploisController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var offreEmploi = await _offresEmploiService.ObtenirOffreEmploiSelonId(id);

            if (offreEmploi.DateFin < DateTime.Now.Date)
                ModelState.AddModelError(string.Empty, "Cette offre d'emploi ne peut plus être supprimée ");

            if (ModelState.IsValid)
            {
                await _offresEmploiService.SupprimerOffreEmploi(id);
                return RedirectToAction(nameof(Index));
            }

            return View(offreEmploi);
        }
    }
}
