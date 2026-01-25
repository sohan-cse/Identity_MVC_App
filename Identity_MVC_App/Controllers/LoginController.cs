using Identity_MVC_App.Helper;
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
                    EmailSender emailSender=new EmailSender();
                    string message = "Dear" + model.Name + "<br/><br/>" +
                        "Thanks for registering with us.<br/><br/>" +
                        "<b>Your user Id:" + model.Email+"<br/>"+"Your Password:"+model.Password+"</b><br/><br/>" +"Regards<br/>" +
                        "<font color='blue' size=10px>Identity MVC App Team</font>";
                    emailSender.SendMail(model.Email, "Registration Successful", message);
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
        [HttpGet]
        public  IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result=await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if(result.Succeeded)
                {
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password...");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            if (_signInManager.IsSignedIn(User))
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login");
            }
            return NotFound();
        }
    }
}
