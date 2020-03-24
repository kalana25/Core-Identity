using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;


namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Authenticate()
        {
            var grandmaClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Bob"),
                new Claim(ClaimTypes.Email,"Bob@email.com"),
                new Claim("Grandmam.says","Very nice boy")
            };

            var licenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Bob K Foo"),
                new Claim("DrivingLicense","A+")
            };

            var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandmam Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government");

            var userPrinciple = new ClaimsPrincipal(new[] { grandmaIdentity });

            HttpContext.SignInAsync(userPrinciple);

            return RedirectToAction("Index");
        }
    }
}
