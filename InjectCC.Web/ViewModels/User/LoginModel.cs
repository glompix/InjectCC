using System.ComponentModel.DataAnnotations;

namespace InjectCC.Web.ViewModels.User
{
    /// <summary>
    /// The login form.
    /// </summary>
    public class LoginModel
    {
        public LoginModel()
        {
            RememberMe = true;
        }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}