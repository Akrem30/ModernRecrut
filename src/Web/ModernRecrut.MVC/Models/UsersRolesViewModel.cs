namespace ModernRecrut.MVC.Models
{
    public class UsersRolesViewModel
    {
        public string UserId { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public string Courriel { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
