using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Identity_Example.Data;
using Microsoft.AspNetCore.Identity;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Identity_Example.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public HomeController(UserManager<IdentityUser> userManger,
            SignInManager<IdentityUser> signManger)
        {
            userManager = userManger;
            signInManager = signManger;
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

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user =await userManager.FindByNameAsync(username);
            if(user!=null)
            {
                var signInResult= await signInManager.PasswordSignInAsync(user, password, false, false);
                if(signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new IdentityUser
            {
                UserName = username,
                Email =""
            };
            var result = await userManager.CreateAsync(user,password);

            if(result.Succeeded)
            {
                var signInResult = await signInManager.PasswordSignInAsync(user, password, false, false);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
