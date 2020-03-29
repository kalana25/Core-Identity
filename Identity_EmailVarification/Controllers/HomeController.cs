using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using NETCore.MailKit.Core;
using Microsoft.AspNetCore.Authorization;
using Identity_EmailVarification.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Identity_EmailVarification.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signinManager;
        private readonly IEmailService emailService;
        private readonly IGmailService gmailService;

        public HomeController(UserManager<IdentityUser> userManger,
            SignInManager<IdentityUser> signManger,
            IGmailService gmalServce,
            IEmailService emalservice)
        {
            userManager = userManger;
            signinManager = signManger;
            emailService = emalservice;
            gmailService = gmalServce;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username,string password)
        {
            var user = new IdentityUser
            {
                Email = "",
                UserName = username
            };

            var result = await userManager.CreateAsync(user, password);
            if(result.Succeeded)
            {
                //generation of email token
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var link = Url.Action(nameof(VarifyEmail), "Home", new { userId = user.Id, code },Request.Scheme,Request.Host.ToString());
                //await emailService.SendAsync("kalana.mahaarachchi@qualitapps.com", "Email Varify",$"<a href=\"{link}\">Varify Email</a>",true);
                gmailService.SendAsync("kalana.mahaarachchi@qualitapps.com", "Email Varify", $"<a href=\"{link}\">Varify Email</a>");
                return RedirectToAction("EmailVarification");
            }
            return RedirectToAction("Index");
        }



        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username,string password)
        {
            var user = await userManager.FindByNameAsync(username);
            if(user!=null)
            {
                var signInResult = await signinManager.PasswordSignInAsync(user, password, false, false);
                if(signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }



        public async Task<IActionResult> Logout()
        {
            await signinManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        public IActionResult EmailVarification ()
        {
            return View();
        }

        public async Task<ActionResult> VarifyEmail(string userId,string code)
        {
            var user = await userManager.FindByIdAsync(userId);
            if(user==null)
            {
                return BadRequest();
            }
            var result = await userManager.ConfirmEmailAsync(user, code);
            if(result.Succeeded)
            {
                return View();
            }
            return BadRequest();
        }
    }
}
