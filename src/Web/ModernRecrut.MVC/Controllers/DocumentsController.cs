using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModernRecrut.MVC.Areas.Identity.Data;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models.DTO;
using System.Security.Claims;

namespace ModernRecrut.MVC.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly IDocumentsService _documentsService;
        private readonly IConfiguration _config;
      
        public DocumentsController(IDocumentsService documentsService, IConfiguration config) 
        {
            _documentsService = documentsService;
            _config = config;         
        }
        // GET: DocumentsController
        public async Task<ActionResult> Index()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var documents = await _documentsService.ObtenirDocumentsSelonId(Guid.Parse(id));
            return View(documents);
        }

        // GET: DocumentsController/Details/5
        public async Task<ActionResult> Details(string nomFichier)
        {
            var lien =await _documentsService.LireFichier(nomFichier.Split(":")[0].Trim());
            return Redirect(lien);
        }

        // GET: DocumentsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DocumentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RequeteFichier fichier)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            fichier.UtilisateurID = Guid.Parse(id);
            if (!fichier.FileDetails.FileName.EndsWith(".pdf") && !fichier.FileDetails.FileName.EndsWith(".docx"))
                ModelState.AddModelError("FileDetails", "Vous pouvez téléchargez juste des fichiers au formats pdf et word");
            if (fichier.FileDetails.Length > _config.GetValue<long>("TailleLimite"))
                ModelState.AddModelError("FileDetails", "Fichier trop volumineux");
            if (ModelState.IsValid)
            {
                await _documentsService.EnregistrerDocument(fichier);

                return RedirectToAction(nameof(Index));             
            }
            return View();        
        }

        // GET: DocumentsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DocumentsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DocumentsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DocumentsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string nomFichier)
        {
            await _documentsService.SupprimerDocument(nomFichier.Split(":")[0].Trim());
            return RedirectToAction(nameof(Index));
        }
    }
}
