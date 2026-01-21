using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Identity_MVC_App.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Please Enter Email")]
        [EmailAddress]
        [DisplayName("Email Id")]
        public string Email {  get; set; }
        [Required(ErrorMessage="Please Enter Password")]
        [DataType(DataType.Password)]
        public string Password {  get; set; }
        [DisplayName("Remember Me")]
        public bool RememberMe { get; set; }
    }
}
