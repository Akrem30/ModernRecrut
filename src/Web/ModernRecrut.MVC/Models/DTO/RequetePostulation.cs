using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModernRecrut.MVC.Models.DTO
{
    public class RequetePostulation
    {
        [Required, DisplayName("Prétention salariales")]
        public decimal PretentionSalariale { get; set; }
        [Required, DisplayName("Date de disponibilité")]
        public DateTime DateDisponibilite { get; set; }
        [Required]
        public Guid CandidatID { get; set; }
        [Required]
        public Guid OffreEmploiID { get; set; }
    }
}
