using Microsoft.VisualBasic.FileIO;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ModernRecrut.Documents.API.Entities.DTO
{
    public class RequeteFichier
    {
        [Required]
        public IFormFile FileDetails { get; set; }
        [Required]
        public Guid UtilisateurID { get; set; }
        [Required]
        public Type TypeDocument { get; set; }
    }
}
