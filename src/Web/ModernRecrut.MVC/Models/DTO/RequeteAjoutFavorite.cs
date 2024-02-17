using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ModernRecrut.MVC.Models.DTO
{
    public class RequeteAjoutFavorite
    {
        [RegularExpression(@"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$",
        ErrorMessage = "La valeur Clé doit être une adresse IP valide.")]
        [DisplayName("Clé")]
        [Required(ErrorMessage = "le champ {0} est obligatoire")]
        public string? Cle { get; set; }
        [Required(ErrorMessage = "Le champ {0} est obligatoire"), DisplayName("Id offre emploi")]
        public Guid OffreEmploiID { get; set; }
    }
}
