using Server.Models;

namespace Server.ViewModels
{
    public class TestsViewModel
    {
        public TestsViewModel(List<Test> tests)
        {
            Tests = tests;
        }

        public List<Test> Tests { get; set; }
    }
}
