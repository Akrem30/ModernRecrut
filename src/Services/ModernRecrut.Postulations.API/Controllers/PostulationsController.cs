using Microsoft.AspNetCore.Mvc;
using ModernRecrut.Postulations.API.Interfaces;
using ModernRecrut.Postulations.API.Entities;
using ModernRecrut.Postulations.API.Entities.DTO;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ModernRecrut.Postulations.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostulationsController : ControllerBase
    {

        private readonly IPostulationsService _postulationsService;
        private string _erreur;
        public PostulationsController(IPostulationsService postulationsService)
        {
            _postulationsService = postulationsService;
        }


        /// <summary>
        /// Obtenir la liste des postulations
        /// </summary>
        /// <response code="200">Liste des postulations obtenues avec succès</response>
        /// <response code="404">Liste des postulations introuvable</response> 
        /// <response code="500">Oops! Le service est indisponible pour le moment</response>
        // GET: api/<PostulationsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var postulations = await _postulationsService.ObtenirPostulations();
            if (postulations == null)
                return NotFound();

            return Ok(postulations);
        }

        /// <summary>
        /// Obtenir une postulation spécifique par son identifiant
        /// </summary>
        /// <param name="id">L'identifiant de la postulation que vous souhaitez obtenir</param>
        /// <response code="200">postulation obtenue avec succès</response>
        /// <response code="404">postulation introuvable pour l'identifiant spécifié</response>    
        /// <response code="500">Oops! Le service est indisponible pour le moment</response>
        // GET api/<PostulationsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var postulation = await _postulationsService.ObtenirPostulationSelonId(id);
            if(postulation == null)
                return NotFound();

            return Ok(postulation);
        }

        /// <summary>
        /// Ajouter une postulation à la base de données 
        /// </summary>
        /// <param name="requetePostulation">La nouvelle postulation à ajouter</param>
        /// <response code="201">postulation ajoutée avec succès</response>
        /// <response code="400">Offre d'emploi ajoutée non valide</response>
        /// <response code="500">Oops! Le service est indisponible pour le moment</response>
        // POST api/<PostulationsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RequetePostulation requetePostulation)
        {
            if (ModelState.IsValid)
            {
                var erreur= await _postulationsService.AjouterPostulation(requetePostulation);
                if (!string.IsNullOrEmpty(erreur))
                    return BadRequest( new { _erreur= erreur });
                
                return NoContent();
            }
            return BadRequest(ModelState);
                
        }

        /// <summary>
        /// Modifer une postulation dans la base de données 
        /// </summary>
        /// <param name="id">L'identifiant de la postulation que vous souhaitez modifier</param>
        /// <response code="204">postulation modifiée avec succès</response>
        /// <response code="400">postulation modifiée non valide</response>
        /// <response code = "500" >Oops! Le service est indisponible pour le moment</response>
        // PUT api/<PostulationsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Postulation postulation)
        {
            if (ModelState.IsValid)
            {
              var erreur=   await _postulationsService.ModifierPostulation(postulation);
                if(!String.IsNullOrEmpty(erreur))
                    return BadRequest(new {_erreur= erreur });

                return NoContent();

            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Supprimer une postulation
        /// </summary>
        /// <param name="id">L'identifiant de la postulation que vous souhaitez supprimer</param>
        /// <response code="204">Suppression de la postulation effectuée avec succès </response>
        /// <response code="400">Erreur lors de la suppression</response> 
        /// <response code="500">Oops! Le service est indisponible pour le moment</response>
        // DELETE api/<PostulationsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var erreur =await _postulationsService.SupprimerPostulation(id);
            if (!string.IsNullOrEmpty(erreur))
            {
                return BadRequest(new {_erreur= erreur });
            }
            return NoContent();
        }
    }
}
