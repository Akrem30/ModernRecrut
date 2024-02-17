using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModernRecrut.MVC.Areas.Identity.Data;
using ModernRecrut.MVC.Models;
using System.Security.Claims;

namespace ModernRecrut.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesUtilisateursController : Controller
    {
        private readonly UserManager<Utilisateur> _gestionnaireUtilisateur;
        private readonly RoleManager<IdentityRole> _gestionnaireRole;
        private readonly ILogger<RolesUtilisateursController> _logger;
        public RolesUtilisateursController(UserManager<Utilisateur> gestionnaireUtilisateur, RoleManager<IdentityRole> gestionnaireRole, ILogger<RolesUtilisateursController> logger)
        {
            _gestionnaireUtilisateur = gestionnaireUtilisateur;
            _gestionnaireRole = gestionnaireRole;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _gestionnaireUtilisateur.Users.ToListAsync();
            var userRolesViewModel = new List<UsersRolesViewModel>();
            foreach (Utilisateur user in users)
            {
                var thisViewModel = new UsersRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Prenom = user.Prenom;
                thisViewModel.Nom = user.Nom;
                thisViewModel.Courriel = user.Email;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }

            _logger.LogInformation(CustomLogEventsRolesUtilisateur.Lecture, $"Lecture des rôles de {users.Count} utilisateurs par l'utilisateur avec l'id {User.FindFirstValue(ClaimTypes.NameIdentifier)}");

            return View(userRolesViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Assigner(string userId)
        {
            ViewBag.userId = userId;
            var user = await _gestionnaireUtilisateur.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Cet utilisateur (Id : {userId}) est introuvable ";
                return NotFound();
            }

            ViewBag.UserNomComplet = user.NomComplet;
            var model = new List<ManageUserRolesViewModel>();

            foreach (var role in _gestionnaireRole.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    NomRole = role.Name
                };

                if (await _gestionnaireUtilisateur.IsInRoleAsync(user, role.Name))
                    userRolesViewModel.Selectionne = true;
                else
                    userRolesViewModel.Selectionne = false;

                model.Add(userRolesViewModel);
            }

            _logger.LogInformation(CustomLogEventsRolesUtilisateur.Lecture, $"Lecture de la page permettant d'attribuer des rôles à l'utilisateur avec id {user.Id} par l'utilisateur avec l'id {User.FindFirstValue(ClaimTypes.NameIdentifier)}");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Assigner(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _gestionnaireUtilisateur.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            if (model.Any(e => (e.NomRole == "Candidat" && e.Selectionne) && model.Any(m => m.NomRole == "Employé" && m.Selectionne)))
            {
                ModelState.AddModelError(string.Empty, "Si un utilisateur est un candidat, il ne peut pas avoir l'autorisation d'employé. " +
                                                       "Si un utilisateur est un employé, il ne peut pas avoir l'autorisation de candidat.");

                return View(model);
            }

            if (model.Any(e => (e.NomRole == "Candidat" && e.Selectionne) && model.Any(m => m.NomRole != "Candidat" && m.Selectionne)))
            {
                ModelState.AddModelError(string.Empty, "Si l'utilisateur est un candidat, il ne peut pas avoir d'autorisations supplémentaires.");
                return View(model);
            }

            if (model.Any(e => (e.NomRole == "Candidat" && !e.Selectionne) && model.Any(m => m.NomRole == "Employé" && !m.Selectionne)))
            {
                ModelState.AddModelError(string.Empty, "Au minimum, un utilisateur doit être autorisé en tant que candidat ou employé.");
                return View(model);
            }

            var roles = await _gestionnaireUtilisateur.GetRolesAsync(user);
            var result = await _gestionnaireUtilisateur.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Ces rôles d'utilisateur ne peuvent pas être supprimés");
                return View(model);
            }

            result = await _gestionnaireUtilisateur.AddToRolesAsync(user, model.Where(x => x.Selectionne).Select(y => y.NomRole));

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Ces rôles sélectionnés ne peuvent pas être ajoutés à l'utilisateur ");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation(CustomLogEventsRolesUtilisateur.Assignation, $"Assignation de rôles à l'utilisateur avec id {user.Id} par l'utilisateur avec l'id {User.FindFirstValue(ClaimTypes.NameIdentifier)}");
                return RedirectToAction("Index");
            }

            return View(model);
        }


        private async Task<List<string>> GetUserRoles(Utilisateur user)
        {
            return new List<string>(await _gestionnaireUtilisateur.GetRolesAsync(user));
        }
    }
}

