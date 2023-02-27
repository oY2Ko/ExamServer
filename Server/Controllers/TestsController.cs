using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.ViewModels;
using System.Data.Entity;
using System.Diagnostics.Eventing.Reader;
using System.IO.Pipelines;

namespace Server.Controllers
{
    [Route("{controller}")]
    public class TestsController : Controller
    {
        private AppDbContext dbContext;
        public TestsController(AppDbContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        [Route("tests")]
        public List<Test> GetTests(string filter)
        {
            var tests = new List<Test>();
            if (string.IsNullOrEmpty(filter))
            {
                tests = dbContext.Tests.ToList();
            }
            else
            {
                tests = dbContext.Tests.Where(x => x.Name == filter).ToList();
            }
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

            dbContext.Tests.Add(new Test() { Name = test.Name, Description = test.Description, Owner = dbContext.Users.First(x => x.Name == owner)});
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("GetTest")]
        public async Task<Test> GetTest([FromHeader]int id)
        {
            var a = dbContext.Tests.FirstOrDefault(x => x.Id == id);
            return a;
        }

    }
}
