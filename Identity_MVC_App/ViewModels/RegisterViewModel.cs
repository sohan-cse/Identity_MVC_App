using System.ComponentModel.DataAnnotations;

namespace Identity_MVC_App.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter Name")]
        public string Name { get; set; }
        [EmailAddress(ErrorMessage = "Please enter correct Email Id")]
        [Display(Name = "Email Id")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter Password")]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [Compare("ConfirmPassword", ErrorMessage = "Password and Confirm Password do not match")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        //[Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        [Required(ErrorMessage = "Please enter Confirm Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmed Password")]
        public string ConfirmPassword { get; set; }
    }
}
