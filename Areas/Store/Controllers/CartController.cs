using FinalExamProject.Infrastructure;
using FinalExamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Areas.Store.Controllers
{
    [Area("Store")]
    public class CartController : Controller
    {
        private readonly GoShopContext _context;
        private const string CART_KEY = "SHOPPING_CAR"; // Session Key

        public CartController(GoShopContext context)
        {
            _context = context;
        }

        // 取得目前 Session 的購物車
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

        // 儲存購物車回 Session
        private void SaveCar(ShoppingCar car)
        {
            HttpContext.Session.SetObject(CART_KEY, car);
        }

        // GET: /Store/Cart
        public IActionResult Index()
        {
            return View(GetCar());
        }

        // POST: /Store/Cart/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Guid id, int qty = 1)
        {
            if (qty < 1) qty = 1;

            var product = await _context.Product
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductID == id && p.IsActive);

            if (product == null)
            {
                TempData["err"] = "商品不存在或已下架。";
                return RedirectToAction("Index", "Catalog", new { area = "Store" });
            }

            var car = GetCar();
            var line = car.Items.FirstOrDefault(x => x.ProductID == id);

            if (line == null)
            {
                car.Items.Add(new ShoppingCarItem
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    Photo = product.Photo,
                    UnitPrice = product.Price,
                    Quantity = qty
                });
            }
            else
            {
                line.Quantity += qty;
            }

            SaveCar(car);
            TempData["ok"] = $"已將「{product.ProductName}」加入購物車。";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Store/Cart/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Guid[] ids, int[] quantities)
        {
            var car = GetCar();

            for (int i = 0; i < ids.Length; i++)
            {
                var line = car.Items.FirstOrDefault(x => x.ProductID == ids[i]);
                if (line != null)
                {
                    var q = Math.Clamp(quantities[i], 1, 999);
                    line.Quantity = q;
                }
            }

            SaveCar(car);
            TempData["ok"] = "數量已更新。";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Store/Cart/Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(Guid id)
        {
            var car = GetCar();
            car.Items.RemoveAll(x => x.ProductID == id);
            SaveCar(car);

            TempData["ok"] = "已移除商品。";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Store/Cart/Clear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Clear()
        {
            var car = GetCar();
            car.Items.Clear();
            SaveCar(car);

            TempData["ok"] = "購物車已清空。";
            return RedirectToAction(nameof(Index));
        }
    }
}
