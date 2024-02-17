using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModernRecrut.MVC.Areas.Identity.Data;
using ModernRecrut.MVC.Models;
using System.Security.Claims;

namespace ModernRecrut.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly UserManager<Utilisateur> _gestionnaireUtilisateur;
        private readonly RoleManager<IdentityRole> _gestionnaireRole;
        private readonly ILogger<RolesController> _logger;

        public RolesController(UserManager<Utilisateur> gestionnaireUtilisateur, RoleManager<IdentityRole> gestionnaireRole, ILogger<RolesController> logger)
        {
            _gestionnaireUtilisateur = gestionnaireUtilisateur;
            _gestionnaireRole = gestionnaireRole;
            _logger = logger;           
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _gestionnaireRole.Roles.ToListAsync();
            _logger.LogInformation(CustomLogEventsRoles.Lecture, $"Lecture de {roles.Count} rôles par l'utilisateur avec l'id {User.FindFirstValue(ClaimTypes.NameIdentifier)}");

            return View(roles);
        }

        public async Task<ActionResult> Details(string id)
        {
            var role = await _gestionnaireRole.FindByIdAsync(id);

            if (role == null)
                return NotFound();

            RoleViewModel roleView = new RoleViewModel()
            {
                RoleId = role.Id,
                NomRole = role.Name,
            };

            _logger.LogInformation(CustomLogEventsRoles.Lecture, $"Lecture des détails pour le rôle {role.Name} avec id {role.Id} par l'utilisateur avec l'id {User.FindFirstValue(ClaimTypes.NameIdentifier)}");

            return View(roleView);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string nomRole)
        {
            if (!String.IsNullOrEmpty(nomRole))
            {
                if (await _gestionnaireRole.Roles.AnyAsync(e => e.Name.ToLower() == nomRole.Trim().ToLower()))
                {
                    ModelState.AddModelError("nomRole", "Ce rôle existe déjà");
                }

                if (ModelState.IsValid)
                {
                    var resultat = await _gestionnaireRole.CreateAsync(new IdentityRole(nomRole.Trim()));

                    if (resultat.Succeeded)
                        _logger.LogInformation(CustomLogEventsRoles.Creation, $"Création du rôle {nomRole} par l'utilisateur avec l'id {User.FindFirstValue(ClaimTypes.NameIdentifier)}");

                    return RedirectToAction("Index");
                }
            }

            return View("Index", await _gestionnaireRole.Roles.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _gestionnaireRole.FindByIdAsync(id);

            if (role == null)
                return NotFound();

            RoleViewModel roleView = new RoleViewModel()
            {
                RoleId = role.Id,
                NomRole = role.Name,
            };

            _logger.LogInformation(CustomLogEventsRoles.Lecture, $"Lecture de la page de modification pour le rôle {role.Name} avec id {role.Id} par l'utilisateur avec l'id {User.FindFirstValue(ClaimTypes.NameIdentifier)}");

            return View(roleView);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel roleView)
        {
            var roleModifie = await _gestionnaireRole.FindByIdAsync(roleView.RoleId);

            if (roleModifie == null)
                return NotFound();

            if (roleModifie.Name == "Admin")
            {
                ModelState.AddModelError("roleAdmin", "Le nom du rôle d'administrateur ne peut pas être modifié.");
                return View(roleView);
            }

            if (ModelState.IsValid)
            {
                roleModifie.Name = roleView.NomRole.Trim();

                var resultat = await _gestionnaireRole.UpdateAsync(roleModifie);

                if (resultat.Succeeded)
                    _logger.LogInformation(CustomLogEventsRoles.Modification, $"Modification du rôle {roleModifie.Name} avec ID {roleModifie.Id} par l'utilisateur avec l'id {User.FindFirstValue(ClaimTypes.NameIdentifier)}");

                return RedirectToAction(nameof(Index));
            }

            return View(roleView);
        }

        // GET: EmploisController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var role = await _gestionnaireRole.FindByIdAsync(id);

            if (role == null)
                return NotFound();

            RoleViewModel roleView = new RoleViewModel()
            {
                RoleId = role.Id,
                NomRole = role.Name,
            };

            _logger.LogInformation(CustomLogEventsRoles.Lecture, $"Lecture de la page de suppression pour le rôle {role.Name} avec id {role.Id} par l'utilisateur avec l'id {User.FindFirstValue(ClaimTypes.NameIdentifier)}");

            return View(roleView);
        }

        // POST: EmploisController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var role = await _gestionnaireRole.FindByIdAsync(id);

            if (role == null)
                return NotFound();

            RoleViewModel roleView = new RoleViewModel()
            {
                RoleId = role.Id,
                NomRole = role.Name,
            };

            if (id == "0d88d65e-266e-463f-9eec-1d314a91baa2")
            {
                ModelState.AddModelError("roleAdmin", "Le rôle d'administrateur ne peut pas être supprimé.");
                return View(roleView);
            }

            if (ModelState.IsValid)
            {
                var resultat = await _gestionnaireRole.DeleteAsync(role);

                if (resultat.Succeeded)
                    _logger.LogInformation(CustomLogEventsRoles.Suppression, $"Suppression du rôle {role.Name} avec id {role.Id} par l'utilisateur avec l'id {User.FindFirstValue(ClaimTypes.NameIdentifier)}");

                return RedirectToAction(nameof(Index));
            }

            return View(roleView);
        }


    }
}