using Identity_MVC_App.Helper;
using Identity_MVC_App.Models;
using Identity_MVC_App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;

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
        [Authorize]
        public async Task<IActionResult>ChangePassword()
        {
            if(_signInManager.IsSignedIn(User))
            {
                var user= await _userManager.Users.FirstOrDefaultAsync(x=>x.Email==_userManager.GetUserName(User));
                if(user!=null)
                {
                    ViewData["email"] = user.UserName;
                }
                return View();
            }
            return RedirectToAction("Login");
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == _userManager.GetUserName(User));

            if (ModelState.IsValid)
            {
                var result =await _userManager.ChangePasswordAsync(user,model.CurrentPassword,model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index","Home");
                }
            }
            if (user != null)
            {
                ViewData["email"] = user.UserName;
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        public async Task SendForgetPasswordEmail(string? email, Users? user)
        {
            //Generate the reset password token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            //Build the password reset link
            var passwordResetLink = Url.Action("ResetPassword", "Login", new {Email=email,Token=token},protocol:HttpContext.Request.Scheme);

            //Encode the link to prevent xss attack
            var safeLink=HtmlEncoder.Default.Encode(passwordResetLink);

            //create the email subject
            var subject = "Reset your password";

            //Creaft html message body
            var messageBody = $@"<div style=""font-family:Arial,Helvetica,sans-serif;font-size:16px;color:#333;line-height:1.5;padding:20px;""><h2 style=""color:#007bff;text-align:center;"">Password reset request</h2><p style=""margin-bottom:20px;"">Hi {user.Name} , </p>
                <p>We received a request to reseet your password for your <strong>Dotnet tutorial</strong> account. If you made this request , please click the button below to reset your password:</p>
                <div style=""text-align:center;margin:20px;"">
                    <a href=""{safeLink}"" style=""background-color:#007bff;color:#fff;padding:10px 20px;text-decoration:none;font-weight:bold;border-radius:5px;display:inline-block;"">Reset Password</a>
                </div> 
                <p>If the button above doesn't work ,copy and paste the followinng URL into your browser:</p>
                <p style=""background-color:#f8f9fa;padding:10px;border:1px solid #ddd;border-radius:5px;""><a href=""{safeLink}"" style=""color:#007bff;text-decoration:none"">{safeLink}</a></p>
                <p>If you didn't request to reset your password, please ignore this email or contact support if you have cooncerns.</p>
                <p style=""margin-top:30px;"">Thank you, <br/>The dot net tutorials team.</p>
                </div>";

            EmailSender emailSender = new EmailSender();
            emailSender.SendMail(email, subject, messageBody);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult>ForgetPassword(ForgetPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user= await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    await SendForgetPasswordEmail(user.Email,user);
                    return RedirectToAction("ForgetPasswordConfirmation");
                }
                else
                {
                    return RedirectToAction("ForgetPassword", "Login");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            if(token == null || email == null)
            {
                ViewBag.ErrorTitle = "Invalid Password Reset Token";
                ViewBag.ErrorMessage = "The link is expired or invalid";
                return View("Error");
            }
            else
            {
                ResetPasswordViewModel model = new ResetPasswordViewModel();
                model.Token = token;
                model.Email = email;
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult>ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    await _userManager.ResetPasswordAsync(user,model.Token,model.Password);
                    return RedirectToAction("ResetPasswordConfirmation");
                }
            }
            return View(model);
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

    }
}
