using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Server.Models;
    using System.Diagnostics;
using System.Security.Claims;

namespace Server.Controllers
{
    [Route("{controller}")]
    public class AccountController : Controller
    {
        private AppDbContext dbContext;
        public AccountController(AppDbContext context)
        {
            dbContext = context;
        }

        [Route("register")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> PostUser([FromForm] string name, string password, string role)
        {
            await dbContext.Users.AddAsync(new User(name, password, role));
            dbContext.SaveChanges();
            await Authenticate(name, role);
            return Ok();
        }

        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

        public class LogInDTO
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LogInPost([FromBody] LogInDTO log)
        {
            var user = dbContext.Users.FirstOrDefault(user => user.Name == log.Login && user.Password == log.Password);
            if (user == null)
            {
                return BadRequest("Неправильные данные входа");
            }
            Authenticate(user.Name, user.Role);
            return Ok();
        }

        private async Task Authenticate(string name, string role)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, name));
            claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, role));
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        }
    }
}