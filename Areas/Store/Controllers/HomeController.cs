using Microsoft.AspNetCore.Mvc;

namespace FinalExamProject.Areas.Store.Controllers
{
    [Area("Store")]
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
