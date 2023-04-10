using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
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
        [HttpGet]
        [Route("logout")]
        [Authorize("Main", Policy = "Admin")]
        public async Task<IActionResult> LogOut()
        {
            HttpContext.Response.Cookies.Delete(".AspNetCore.Main");
            await HttpContext.SignOutAsync("Main");
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
            var a = Request.Cookies;
            
            var user = dbContext.Users.FirstOrDefault(user => user.Name == log.Login && user.Password == log.Password);
            if (user == null)
            {
                return BadRequest("Неправильные данные входа");
            }
            //Response.Cookies.Append(ClaimsIdentity.DefaultRoleClaimType, "Admin", new CookieOptions() { Secure = true, SameSite = SameSiteMode. });
            await Authenticate(user.Name, user.Role);
            var useyyr = User;
            return Ok();
        }

        public async Task Authenticate(string name, string role)
        {
            var claims = new List<Claim>
            {
                new Claim("MyName", name),
                new Claim("MyRole", "Admin")
            };
            var identity = new ClaimsIdentity(claims, "Cookie");
            await HttpContext.SignInAsync("Main", new ClaimsPrincipal(identity), new AuthenticationProperties(){ IsPersistent = true });
        }
    }
}