﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Views;
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
        [HttpGet]
        public IActionResult GetRegisterPage()
        {
            return View("../RegisterView");
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

        [Route("login")]
        [HttpGet]
        public async Task<IActionResult> LogIn()
        {
            return View("../LogInView");
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LogIn([FromForm]string name, string password)
        {
            var user = dbContext.Users.FirstOrDefault(user => user.Name == name && user.Password == password);
            if (user == null)
            {
                return BadRequest("Неправильные данные входа");
            }
            Authenticate(user.Name, user.Role);
            return Ok();
        }

        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LogInPost([FromForm] string name, string password)
        {
            var user = dbContext.Users.FirstOrDefault(user => user.Name == name && user.Password == password);
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