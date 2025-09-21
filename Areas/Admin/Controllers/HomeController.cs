using FinalExamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly GoShopContext _ctx;
        public HomeController(GoShopContext ctx) => _ctx = ctx;

        // GET: /Admin/Home/Index
        public async Task<IActionResult> Index()
        {
            var vm = new AdminDashboardVm
            {
                ProductCount = await _ctx.Product.CountAsync(),
                CategoryCount = await _ctx.Category.CountAsync(),
                OrderCount = await _ctx.Order.CountAsync(),
                MemberCount = await _ctx.Member.CountAsync(),
                InventoryLowCount = await _ctx.Inventory.CountAsync(i => i.Quantity < i.SafetyStock),
                PendingShipment = await _ctx.Shipping.CountAsync(s => s.Status == "Processing"),
                PaymentPending = await _ctx.Payment.CountAsync(p => p.Status == "Pending"),
            };

            return View(vm);
        }
    }
}
