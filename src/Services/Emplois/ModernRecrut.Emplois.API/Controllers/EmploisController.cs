using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModernRecrut.Emplois.Core.DTO;
using ModernRecrut.Emplois.Core.Entites;
using ModernRecrut.Emplois.Core.Interfaces;

namespace ModernRecrut.Emplois.API.Controllers
{
    [Route("api/[controller]")]
    public class EmploisController : MainController
    {
        private readonly IEmploiService _emploisService;

        public EmploisController(IEmploiService emploisService)
        {
            _emploisService = emploisService;
        }

        /// <summary>
        /// Obtenir la liste de tous les offres d'emploi
        /// </summary>
        /// <response code="200">Liste de tous les offres d'emploi obtenus avec succès</response>
        /// <response code="404">Liste de tous les offres d'emploi introuvable</response> 
        /// <response code="500">Oops! Le service est indisponible pour le moment</response>
        // GET: api/<EmploisController>
        [HttpGet]
        public async Task<IEnumerable<Emploi>> Get()
        {
            return await _emploisService.ObtenirTout();
        }

        /// <summary>
        /// Obtenir la liste de tous les offres d'emploi valides
        /// </summary>
        /// <remarks>
        /// Les offres d'emploi valides sont celles dont la date d'affichage est inférieure ou égale à la date du jour et 
        /// dont la date de fin est supérieure ou égale à la date du jour.
        /// </remarks>
        /// <response code="200">Liste de tous les offres d'emploi valides obtenus avec succès</response>
        /// <response code="404">Liste de tous les offres d'emploi valides introuvable</response> 
        /// <response code="500">Oops! Le service est indisponible pour le moment</response>
        // GET: api/<EmploisController>
        [HttpGet("EmploisValide")]
        public async Task<IEnumerable<Emploi>> EmploisValide()
        {
            return (await _emploisService.ObtenirTout()).Where(e => e.DateAffichage <= DateTime.Now.Date && e.DateFin >= DateTime.Now.Date);
        }

        /// <summary>
        /// Obtenir une offre d'emploi spécifique par son identifiant
        /// </summary>
        /// <param name="id">L'identifiant de l'offre d'emploi que vous souhaitez obtenir</param>
        /// <response code="200">Offre d'emploi obtenu avec succès</response>
        /// <response code="404">Offre d'emploi introuvable pour l'identifiant spécifié</response>    
        /// <response code="500">Oops! Le service est indisponible pour le moment</response>
        // GET api/<EmploisController>/5
        [HttpGet("{id}")]
        public async Task<Emploi> Get(Guid id)
        {
            return await _emploisService.ObtenirSelonId(id);
        }

        /// <summary>
        /// Ajouter une offre d'emploi à la base de données 
        /// </summary>
        /// <param name="requeteOffreEmploi">La nouvelle offre d'emploi à ajouter</param>
        /// <response code="201">Offre d'emploi ajouté avec succès</response>
        /// <response code="400">Offre d'emploi ajouté non valide</response>
        /// <response code="500">Oops! Le service est indisponible pour le moment</response>
        // POST api/<EmploisController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RequeteOffreEmploi requeteOffreEmploi)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _emploisService.Ajouter(requeteOffreEmploi);

            return CustomResponse(requeteOffreEmploi);
        }

        // PUT api/<EmploisController>/5
        /// <summary>
        /// Modifer une offre d'emploi dans la base de données 
        /// </summary>
        /// <param name="id">L'identifiant de l'offre d'emploi que vous souhaitez modifier</param>
        /// <response code="204">Offre d'emploi modifié avec succès</response>
        /// <response code="400">Offre d'emploi modifié non valide</response>
        /// <response code = "500" >Oops! Le service est indisponible pour le moment</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Emploi emploi)
        {
            if (id != emploi.Id)
            {
                return BadRequest();
            }

            await _emploisService.Modifier(emploi);
            return NoContent();
        }

        /// <summary>
        /// Supprimer une offre d'emploi
        /// </summary>
        /// <param name="id">L'identifiant de l'offre d'emploi que vous souhaitez supprimer</param>
        /// <response code="204">Suppression de l'offre d'emploi effectuée avec succès </response>
        /// <response code="400">L'emprunt ne peut être supprimé car le livre a déjà été retourné</response>
        /// <response code="404">Offre d'emploi introuvable</response> 
        /// <response code="500">Oops! Le service est indisponible pour le moment</response>
        // DELETE api/<EmploisController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var emploi = await _emploisService.ObtenirSelonId(id);

            if (emploi == null) return NotFound();

            await _emploisService.Supprimer(emploi);

            return CustomResponse();
        }
    }
}