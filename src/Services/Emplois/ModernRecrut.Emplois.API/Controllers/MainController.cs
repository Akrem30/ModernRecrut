using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ModernRecrut.Emplois.API.Controllers
{
    
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        protected ICollection<string> Errors = new List<string>();

        protected ActionResult CustomResponse(object result = null)
        {
            if (OperationValide())
            {
                return Ok(result);
            }

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Messages" , Errors.ToArray() }
            }));
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            foreach (var error in errors)
            {
                AjouterErreurTraitement(error.ErrorMessage);
            } 
            return CustomResponse();
        }

        protected bool OperationValide()
        {
            return !Errors.Any();
        }

        protected void AjouterErreurTraitement(string error)
        {
            Errors.Add(error);
        }
        protected void ClaireErreurTraitement()
        {
            Errors.Clear();
        }
    }
}
