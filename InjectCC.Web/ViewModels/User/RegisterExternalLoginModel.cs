using System.ComponentModel.DataAnnotations;

namespace InjectCC.Web.ViewModels.User
{
    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string ExternalLoginData { get; set; }
    }
}