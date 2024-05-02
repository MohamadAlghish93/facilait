using System.ComponentModel.DataAnnotations;

namespace FacilaIT.Models.DTO{
    public class LoginModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        public bool ExternalSP { get; set; }

        public string ExternalFilter { get; set; }
    }
}