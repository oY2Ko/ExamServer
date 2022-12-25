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
        [Route("Add")]
        public IActionResult Add([FromForm] string name, string )
        {
            return View();
        }

        [HttpGet]
        [Route("tests")]
        public IActionResult GetTests(string filter)
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
            return View("../TestsView", new TestsViewModel(tests));
        }
    }
}
