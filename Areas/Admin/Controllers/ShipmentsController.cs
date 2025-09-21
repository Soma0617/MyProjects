using FinalExamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShipmentsController : Controller
    {
        private readonly GoShopContext _db;
        public ShipmentsController(GoShopContext db) => _db = db;

        // GET: /Admin/Shipments
        // 列出所有待出貨訂單（ShippingStatus = "Pending"）
        public async Task<IActionResult> Index()
        {
            var list = await _db.Order
                .AsNoTracking()
                .Where(o => o.ShippingStatus == "Pending")
                .OrderByDescending(o => o.CreatedDate)
                .Select(o => new ShipmentRowVm
                {
                    OrderID = o.OrderID,
                    OrderNumber = o.OrderNumber,
                    CreatedDate = o.CreatedDate,
                    RecipientName = o.RecipientName,
                    RecipientPhone = o.RecipientPhone,
                    RecipientAddress = o.RecipientAddress
                })
                .ToListAsync();

            return View(list);
        }

        // GET: /Admin/Shipments/Ship/{id}
        public async Task<IActionResult> Ship(Guid id)
        {
            var order = await _db.Order.AsNoTracking().FirstOrDefaultAsync(o => o.OrderID == id);
            if (order == null) return NotFound();

            if (!string.Equals(order.ShippingStatus, "Pending", StringComparison.OrdinalIgnoreCase))
            {
                TempData["err"] = "此訂單目前狀態不可出貨。";
                return RedirectToAction(nameof(Index));
            }

            var vm = new ShipOrderVm
            {
                OrderID = order.OrderID,
                OrderNumber = order.OrderNumber,
                RecipientName = order.RecipientName,
                RecipientAddress = order.RecipientAddress,
                RecipientPhone = order.RecipientPhone
            };
            return View(vm);
        }

        // POST: /Admin/Shipments/Ship/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ship(Guid id, ShipOrderVm vm)
        {
            if (id != vm.OrderID) return NotFound();

            var order = await _db.Order.FirstOrDefaultAsync(o => o.OrderID == id);
            if (order == null) return NotFound();

            if (!string.Equals(order.ShippingStatus, "Pending", StringComparison.OrdinalIgnoreCase))
            {
                TempData["err"] = "此訂單目前狀態不可出貨。";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid) return View(vm);

            // 建立一筆 Shipping（你的 Shipping 模型欄位請以實際為準）
            var shipping = new Shipping
            {
                // 假設模型有這些欄位（你之前的 Fluent 設定已見到）
                OrderID = order.OrderID,
                TrackingNumber = vm.TrackingNumber!.Trim(),
                Carrier = vm.Carrier!.Trim(),
                RecipientName = order.RecipientName,
                ShippingAddress = order.RecipientAddress,
                RecipientPhone = order.RecipientPhone
            };
            _db.Shipping.Add(shipping);

            // 更新訂單狀態
            order.ShippingStatus = "Shipped";
            order.UpdatedDate = DateTime.Now;

            await _db.SaveChangesAsync();

            TempData["ok"] = $"訂單 {order.OrderNumber} 已出貨。";
            return RedirectToAction(nameof(Index));
        }
    }

    // 列表用
    public class ShipmentRowVm
    {
        public Guid OrderID { get; set; }
        public string OrderNumber { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string RecipientName { get; set; } = null!;
        public string RecipientPhone { get; set; } = null!;
        public string RecipientAddress { get; set; } = null!;
    }

    // 出貨表單用
    public class ShipOrderVm
    {
        public Guid OrderID { get; set; }
        public string OrderNumber { get; set; } = null!;

        // 顯示用（readonly）
        public string RecipientName { get; set; } = null!;
        public string RecipientAddress { get; set; } = null!;
        public string RecipientPhone { get; set; } = null!;

        // 輸入欄位
        [System.ComponentModel.DataAnnotations.Required, System.ComponentModel.DataAnnotations.StringLength(50)]
        public string? Carrier { get; set; }

        [System.ComponentModel.DataAnnotations.Required, System.ComponentModel.DataAnnotations.StringLength(30)]
        public string? TrackingNumber { get; set; }
    }
}
