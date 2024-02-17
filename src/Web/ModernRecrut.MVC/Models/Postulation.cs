using ModernRecrut.MVC.Areas.Identity.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModernRecrut.MVC.Models
{
    public class Postulation
    {
        public Guid Id { get; set; }
        [DisplayName("Prétention salariales")]
        public decimal PretentionSalariale { get; set; }
        [DisplayName("Date de disponibilité")]
        public DateTime DateDisponibilite { get; set; }
        public Guid CandidatID { get; set; }
        public Guid OffreEmploiID { get; set; }

        public ICollection<Note>? Notes { get; set; }

        public Utilisateur? Candidat {  get; set; }
        public OffreEmploi? OffreEmploi { get; set; }
    }
}
