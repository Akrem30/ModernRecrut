using ModernRecrut.Postulations.API.Entities;
using System.ComponentModel.DataAnnotations;

namespace ModernRecrut.Postulations.API.Entities.DTO
{
    public class RequetePostulation
    {
        [Required]
        public decimal PretentionSalariale { get; set; }
        [Required]
        public DateTime DateDisponibilite { get; set; }
        [Required]
        public Guid CandidatID { get; set; }
        [Required]
        public Guid OffreEmploiID { get; set; }
        
    }
}
