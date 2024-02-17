using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.DTO;
using ModernRecrut.MVC.Services;
using System.Net;

namespace ModernRecrut.MVC.Controllers
{
    public class FavorisController : Controller
    {
        private readonly IFavorisService _favorisService;
        private readonly IOffresEmploiService _offresEmploiService;
        private string ipAddress;
        public FavorisController(IFavorisService favorisService, IOffresEmploiService offresEmploiService)
        {
            _favorisService = favorisService;
            _offresEmploiService = offresEmploiService;
        }

        // GET: FavorisController
        public async Task<ActionResult> Index()
        {
            RecupererAdresseIp();

            Favoris favoris = await _favorisService.ObtenirTout(ipAddress);
            return View(favoris);
        }

        public async Task<ActionResult> Details(Guid id)
        {
            var offreEmploi = await _offresEmploiService.ObtenirOffreEmploiSelonId(id);
            return View(offreEmploi);
        }

        // POST: FavorisController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OffreEmploi offre)
        {
            RecupererAdresseIp();

            if (ModelState.IsValid)
            {
                RequeteAjoutFavorite requeteFavorite = new RequeteAjoutFavorite { Cle = ipAddress, OffreEmploiID = offre.Id };
                await _favorisService.AjouterFavorite(requeteFavorite);
            }
            
            return RedirectToAction("Index", "OffresEmploi");
        }

        // GET: FavorisController/Delete/5
        public async Task<ActionResult> Delete(Guid id)
        {
            RecupererAdresseIp();

            var favoris = await _favorisService.ObtenirTout(ipAddress);
            var offreASupprimer = favoris.Contenu.FirstOrDefault(o => o.Id == id);
            return View(offreASupprimer);
        }

        // POST: FavorisController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            RecupererAdresseIp();

            if (ModelState.IsValid)
            {
                await _favorisService.SupprimerFavorite(ipAddress, id);
            }

            return RedirectToAction(nameof(Index));
        }

        private void RecupererAdresseIp()
        {
            ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            //En local, on obtient toujours l'adresse loopback IPv6 compressée
            if (ipAddress == "::1")
                ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].MapToIPv4().ToString();

        }
    }
}

