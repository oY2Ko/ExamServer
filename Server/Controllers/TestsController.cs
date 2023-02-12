using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.ViewModels;
using System.Data.Entity;
using System.Diagnostics.Eventing.Reader;

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

        [HttpPost]
        [Route("AddTest")]
        public IActionResult AddTest([FromBody]string name,[FromBody]string description)
        {
            dbContext.Tests.Add(new Test() { Name = name, Description = description});
            var a = Request;
            dbContext.SaveChanges();
            return Ok();
        }

    }
}
