using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ModernRecrut.MVC.Models
{
    public class Favoris
    {
        [Key]
        [DisplayName("Id du compte de favoris")]
        public string Cle { get; set; }

        [DisplayName("Liste de mes favoris")]
        public List<OffreEmploi>? Contenu { get; set; }
    }
}
