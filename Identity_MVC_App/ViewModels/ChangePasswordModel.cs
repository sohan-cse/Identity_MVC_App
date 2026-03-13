using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Identity_MVC_App.ViewModels
{
    public class ChangePasswordModel
    {
        [EmailAddress(ErrorMessage = "Please enter correct Email Id")]
        [Display(Name = "Email Id")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Current Password")]
        [DataType(DataType.Password)]
        [DisplayName("Current Password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Please enter new Password")]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [Compare("ConfirmPassword", ErrorMessage = "Password and Confirm Password do not match")]
        [DataType(DataType.Password)]
        [DisplayName("New Password")]
        public string NewPassword { get; set; }
        //[Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        [Required(ErrorMessage = "Please enter Confirm Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        public string ConfirmPassword { get; set; }
    }
}
