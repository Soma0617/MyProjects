using FinalExamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MembersController : Controller   // 注意：是 Members（複數）
    {
        private readonly GoShopContext _context;
        public MembersController(GoShopContext context) => _context = context;

        // GET: /Admin/Members
        public async Task<IActionResult> Index()
        {
            var members = await _context.Member.AsNoTracking().ToListAsync();
            return View(members);
        }
    }
}
