using System.ComponentModel.DataAnnotations;

namespace ModernRecrut.MVC.Models
{
    public class RoleViewModel
    {
        [Display(Name = "Identifiant")]
        public string RoleId { get; set; }

        [Display(Name = "Nom")]
        [Required(ErrorMessage = "Le champ {0} est obligatoire")]
        public string NomRole { get; set; }
    }
}
