using Identity_MVC_App.Models;
using Identity_MVC_App.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity_MVC_App.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<Users>_userManager;
        private readonly SignInManager<Users>_signInManager;

        public LoginController(UserManager<Users> userManager, SignInManager<Users> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                Users user = new Users()
                {
                    Name = model.Name,
                    Email = model.Email,
                    NormalizedEmail = model.Email,
                    UserName=model.Email,
                    NormalizedUserName=model.Email
                };
                var result= await _userManager.CreateAsync(user,model.Password);

                if(result.Succeeded)
                {
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                }
            }
            return View(model);
        }
    }
}
