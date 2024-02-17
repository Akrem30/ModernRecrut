using System.ComponentModel.DataAnnotations;

namespace ModernRecrut.MVC.Models.DTO
{
    public class RequeteNote
    {
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? NomEmetteur { get; set; }
        [Required]
        public Guid PostulationID { get; set; }
    }
}
