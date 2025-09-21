using FinalExamProject.Infrastructure;
using FinalExamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Areas.Store.Controllers
{
    [Area("Store")]
    public class CheckoutController : Controller
    {
        private readonly GoShopContext _context;
        private const string CART_KEY = "SHOPPING_CAR";

        public CheckoutController(GoShopContext context)
        {
            _context = context;
        }

        private ShoppingCar GetCar()
        {
            var car = HttpContext.Session.GetObject<ShoppingCar>(CART_KEY);
            if (car == null)
            {
                car = new ShoppingCar();
                HttpContext.Session.SetObject(CART_KEY, car);
            }
            return car;
        }

        // GET: /Store/Checkout
        [HttpGet]
        public IActionResult Index()
        {
            // 必須登入
            var sid = HttpContext.Session.GetString("LOGIN_MEMBER_ID");
            if (!Guid.TryParse(sid, out _))
            {
                var returnUrl = Url.Action("Index", "Checkout", new { area = "Store" });
                return RedirectToAction("Login", "Account", new { area = "Store", returnUrl });
            }

            var car = GetCar();
            if (!car.Items.Any())
            {
                TempData["err"] = "購物車是空的";
                return RedirectToAction("Index", "Cart", new { area = "Store" });
            }

            return View(new CheckoutVm());
        }

        // POST: /Store/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CheckoutVm vm)
        {
            // 讀 Session 取得目前登入會員
            var sid = HttpContext.Session.GetString("LOGIN_MEMBER_ID");
            if (!Guid.TryParse(sid, out var memberId))
            {
                var returnUrl = Url.Action("Index", "Checkout", new { area = "Store" });
                return RedirectToAction("Login", "Account", new { area = "Store", returnUrl });
            }

            var car = GetCar();
            if (!car.Items.Any())
            {
                TempData["err"] = "購物車是空的";
                return RedirectToAction("Index", "Cart", new { area = "Store" });
            }

            if (!ModelState.IsValid) return View(vm);

            // 建立訂單
            var order = new Order
            {
                OrderID = Guid.NewGuid(),
                OrderNumber = $"O{DateTime.Now:yyyyMMddHHmmssfff}",
                MemberID = memberId,                               // ★★ 就是這裡：一定使用 Session 內的會員 ★★
                RecipientName = vm.RecipientName.Trim(),
                RecipientPhone = vm.RecipientPhone.Trim(),
                RecipientAddress = vm.RecipientAddress.Trim(),
                PaymentMethod = vm.PaymentMethod,
                PaymentStatus = "Unpaid",
                ShippingStatus = "Pending",
                OrderStatus = "Active",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            _context.Order.Add(order);

            // 明細
            foreach (var i in car.Items)
            {
                var detail = new OrderDetail
                {
                    OrderDetailID = Guid.NewGuid(),
                    OrderID = order.OrderID,
                    ProductID = i.ProductID,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                };
                _context.OrderDetail.Add(detail);

                // 若要扣庫存可在此處更新 Inventory
                // var inv = await _context.Inventory.FirstOrDefaultAsync(x => x.ProductID == i.ProductID);
                // if (inv != null) { inv.Quantity = Math.Max(0, inv.Quantity - i.Quantity); }
            }

            await _context.SaveChangesAsync();

            // 清空購物車
            car.Items.Clear();
            HttpContext.Session.SetObject(CART_KEY, car);

            TempData["ok"] = $"訂單建立成功，訂單編號 {order.OrderNumber}";
            return RedirectToAction("Success", new { id = order.OrderID });
        }

        // GET: /Store/Checkout/Success/{id}
        [HttpGet]
        public async Task<IActionResult> Success(Guid id)
        {
            var order = await _context.Order
                .Include(o => o.MemberInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null) return NotFound();

            var details = await _context.OrderDetail
                .Include(od => od.ProductInfo)
                .AsNoTracking()
                .Where(od => od.OrderID == id)
                .ToListAsync();

            ViewBag.Details = details;
            return View(order);
        }
    }
}
