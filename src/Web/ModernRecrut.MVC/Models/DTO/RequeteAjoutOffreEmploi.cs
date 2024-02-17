using System.ComponentModel.DataAnnotations;

namespace ModernRecrut.MVC.Models.DTO
{
    public class RequeteAjoutOffreEmploi
    {
        [Required(ErrorMessage = "Le champ {0} est obligatoire")]
        [Display(Name = "Date d'affichage")]
        [DataType(DataType.Date)]
        public DateTime DateAffichage { get; set; }

        [Required(ErrorMessage = "Le champ {0} est obligatoire")]
        [Display(Name = "Date de fin de l'affichage")]
        [DataType(DataType.Date)]
        public DateTime DateFin { get; set; }

        [Required(ErrorMessage = "Le champ {0} est obligatoire")]
        [MaxLength(500, ErrorMessage = "La taille maximale du champ {0} est de {1} caractères")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Le champ {0} est obligatoire")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "La taille maximale du champ est de {1} caractères")]
        public string Poste { get; set; }
    }
}
