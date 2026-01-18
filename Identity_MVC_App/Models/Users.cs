using Microsoft.AspNetCore.Identity;

namespace Identity_MVC_App.Models
{
    public class Users:IdentityUser
    {
        public string Name { get; set; }
    }
}
