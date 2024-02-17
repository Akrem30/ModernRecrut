using Microsoft.AspNetCore.Mvc;
using ModernRecrut.Favoris.API.Interfaces;
using ModernRecrut.Favoris.API.Models.DTO;

namespace ModernRecrut.Favoris.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavorisController : ControllerBase
    {
        private IFavorisService _favorisService;
        private List<string> _erreurs;
        public FavorisController(IFavorisService favorisService)
        {

            _favorisService = favorisService;
            _erreurs = new List<string>();


        }
        /// <summary>
        /// Obtenir La liste de favoris d'un utilisateur par son adresse ip
        /// </summary>
        /// <param name="ipAddress">L'adresse ip de l'utilisateur dont on retourne sa liste de favoris</param>
        /// <response code="200">Liste de favoris obtenue avec succès</response>
        /// <response code="400">Erreur dans l'adresse ip saisie</response>       
        // GET: api/<FavorisController>
        [HttpGet("{ipAddress}")]
        public async Task<IActionResult> Get(string ipAddress)
        {
            Models.Favoris favoris = await _favorisService.ObtenirTout(ipAddress);

            if (favoris != null)
                return Ok(favoris);

            else
                return BadRequest("L'adresse ip est invalide");

        }


        /// <summary>
        /// Ajouter une offre d'emploi à la liste des favoris d'un utilisateur 
        /// </summary>
        /// <remarks>l'identifient de l'offre et l'adresse ip de l'utilisateur sont nécessaires pour effectuer l'ajout </remarks>  
        /// <param name="requeteFavoris">Le nouvel ajout dans les favoris</param>
        /// <response code="201">offre ajouté aux favoris avec succès</response>
        /// <response code="400">offre ou adresse ip non valides</response>
        // POST api/<FavorisController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] RequeteFavoris requeteFavoris)
        {
            if (ModelState.IsValid)
            {
                _erreurs = await _favorisService.Ajouter(requeteFavoris);
                if (_erreurs.Any())
                    return BadRequest(new { erreurs = _erreurs });

                return CreatedAtAction("Post", requeteFavoris);
            }
            return BadRequest("Erreur lors de l'ajout");
        }


        /// <summary>
        /// Supprimer une offre d'emploi de sa liste des favoris
        /// </summary>
        /// <param name="ipAddress">adresse ip du client qui supprime l'offre de sa liste de favoris</param>
        /// <param name="offreId">id de l'offre d'emploi à supprimer</param>
        /// <response code="204">Suppression de l'offre de la liste des favoris effectuée avec succès </response>
        /// <response code="400">Erreur lors de la suppression de l'offre de la liste des favoris</response>
        // DELETE api/<FavorisController>/5
        [HttpDelete("{ipAddress}/{offreId}")]
        public async Task<ActionResult> Delete(string ipAddress, Guid offreId)
        {
            _erreurs = await _favorisService.Supprimer(ipAddress, offreId);
            if (_erreurs.Any())
                return BadRequest(new { erreurs = _erreurs });

            return NoContent();

        }
    }
}
