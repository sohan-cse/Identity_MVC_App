using System.ComponentModel.DataAnnotations;

namespace Identity_MVC_App.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage ="Please enter your email.")]
        [EmailAddress(ErrorMessage ="The email is not valid.")]
        public string Email { get; set; }
    }
}
