using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModernRecrut.MVC.Models.DTO
{
    public class RequeteFichier
    {
        [Required(ErrorMessage ="Vous devez téléchargez un fichier")]      
        public IFormFile FileDetails { get; set; }
       
        public Guid UtilisateurID { get; set; }

        [Required(ErrorMessage ="Vous devez choisir le type du document")]
        [DisplayName("Type du document")]
        public Type TypeDocument { get; set; }
    }
}
