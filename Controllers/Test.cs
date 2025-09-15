using Microsoft.AspNetCore.Mvc;

namespace FinalExamProject.Controllers
{
    public class Test : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
