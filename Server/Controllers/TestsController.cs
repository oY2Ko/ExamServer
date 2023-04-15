using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using System.Security.Claims;

namespace Server.Controllers
{

    [Route("{controller}")]
    [Authorize("Main", Policy = "Admin")]
    public class TestsController : Controller
    {
        private AppDbContext dbContext;
        public TestsController(AppDbContext context)
        {
            dbContext = context;
        }

        public class tt
        {
            public int Id { get; set; }
        }

        [HttpDelete]
        [Route("DeleteTest")]
        public async Task<IActionResult> DeleteTest([FromHeader] int id)
        {
            var a = HttpContext;
            var test = dbContext.Tests.FirstOrDefault(x => x.Id == id);
            if (test == null)
            {
                return BadRequest("No such test");
            }
            dbContext.Tests.Remove(test);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("tests")]
        [AllowAnonymous]
        public async Task<List<Test>> GetTests(string filter)
        {
            var tests = new List<Test>();
            if (string.IsNullOrEmpty(filter))
            {
                tests = dbContext.Tests.ToList();
            }
            else
            {
                tests = dbContext.Tests.Where(x => x.Name.ToLower().Contains(filter.ToLower())).ToList();
            }


            var us = User;

            return tests;
        }
        [HttpPost]
        [Route("SetActivation")]
        public IActionResult SetTestActivation(bool isActive, int testId)
        {

            var test = dbContext.Tests.FirstOrDefault(x => x.Id == testId);
            if (test != null)
            {
                test.IsActive = isActive;
                dbContext.SaveChanges();
                return Ok();
            }
            else
            {
                return Problem("There is no such test");
            }

        }

        public class TestDTO
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }

        [HttpPost]
        [Route("AddTest")]
        public IActionResult AddTest([FromBody]TestDTO test, [FromBody] string owner = "DefaultUser")
        {
            if (string.IsNullOrEmpty(test.Name))
            {
                return BadRequest("Empty Name");
            }
            dbContext.Tests.Add(new Test() { Name = test.Name, Description = test.Description, Questions = new List<Question>(), Owner = dbContext.Users.First(x => x.Name == "DefaultUser")});
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("GetTest")]
        public Test GetTest([FromHeader]int id)
        {
            var test = dbContext.Tests.Include(p => p.Questions).First(x => x.Id == id);
            for (int i = 0; i < test.Questions.Count; i++)
            {
                test.Questions[i].Test = null;
            }
            return test;
            }

        [HttpPost]
        [Route("UpdateTest")]
        public IActionResult UpdateTest([FromBody] Test test)
        {
            if (test.Questions.Any(x => 
            {

                return string.IsNullOrEmpty(x.Text) || string.IsNullOrEmpty(x.Answer) || x.Mark == default(double)
                || string.IsNullOrWhiteSpace(x.Text) || string.IsNullOrWhiteSpace(x.Answer);
            }))
            {
                return BadRequest("Question's got empty fields");
            }
            
            try
            { 
                dbContext.Update(test);
                dbContext.SaveChanges();
                return Ok();    
            }
            catch (Exception)
            {

                return BadRequest("There is no such test");
            }
        }

        [HttpDelete]
        [Route("DeleteQuestion")]
        public IActionResult DeleteQuestion([FromHeader] int id)
        {
            dbContext.Questions.Remove(dbContext.Questions.First(x => x.Id == id));
            dbContext.SaveChanges();
            return Ok();
        }
    }
}
