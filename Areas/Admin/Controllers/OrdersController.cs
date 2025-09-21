using FinalExamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly GoShopContext _context;

        public OrdersController(GoShopContext context)
        {
            _context = context;
        }

        // ====== 小型 ViewModel（為了不動你的實體）======

        // 列表用
        public class OrderListItemVm
        {
            public Guid OrderID { get; set; }
            public string OrderNumber { get; set; } = null!;
            public string? MemberName { get; set; }
            public DateTime CreatedDate { get; set; }   // ← 用 CreatedDate
            public decimal TotalAmount { get; set; }
            public string PaymentStatus { get; set; } = null!;
            public string ShippingStatus { get; set; } = null!;
            public string OrderStatus { get; set; } = null!;
        }

        // 詳細頁用
        public class OrderDetailRowVm
        {
            public string ProductName { get; set; } = null!;
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal Subtotal => Quantity * UnitPrice;
        }

        public class OrderDetailsPageVm
        {
            public Order Order { get; set; } = null!;
            public List<OrderDetailRowVm> Details { get; set; } = new();
            public decimal TotalAmount => Details.Sum(d => d.Subtotal);
        }

        // 編輯頁用
        public class OrderEditVm
        {
            public Guid OrderID { get; set; }
            public string OrderNumber { get; set; } = null!;
            public DateTime CreatedDate { get; set; }

            public string RecipientName { get; set; } = null!;
            public string RecipientPhone { get; set; } = null!;
            public string RecipientAddress { get; set; } = null!;

            public string? PaymentMethod { get; set; }
            public string PaymentStatus { get; set; } = null!;
            public string ShippingStatus { get; set; } = null!;
            public string OrderStatus { get; set; } = null!;
        }

        // ====== Index ======
        public async Task<IActionResult> Index(string? q, string? payment, string? shipping, string? status, int page = 1, int pageSize = 20)
        {
            // 先把每筆訂單的總額查出成字典
            var totals = await _context.OrderDetail
                .GroupBy(d => d.OrderID)
                .Select(g => new { OrderID = g.Key, Total = g.Sum(x => x.UnitPrice * x.Quantity) })
                .ToDictionaryAsync(x => x.OrderID, x => x.Total);

            var query = _context.Order
                .Include(o => o.MemberInfo)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var key = q.Trim();
                query = query.Where(o =>
                    o.OrderNumber.Contains(key) ||
                    o.RecipientName.Contains(key) ||
                    o.RecipientPhone.Contains(key) ||
                    o.RecipientAddress.Contains(key) ||
                    (o.MemberInfo != null && o.MemberInfo.Name.Contains(key)));
            }

            if (!string.IsNullOrWhiteSpace(payment))
                query = query.Where(o => o.PaymentStatus == payment);

            if (!string.IsNullOrWhiteSpace(shipping))
                query = query.Where(o => o.ShippingStatus == shipping);

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(o => o.OrderStatus == status);

            // 用 CreatedDate 由新到舊
            query = query.OrderByDescending(o => o.CreatedDate);

            var total = await query.CountAsync();
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 10, 100);

            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new OrderListItemVm
                {
                    OrderID = o.OrderID,
                    OrderNumber = o.OrderNumber,
                    MemberName = o.MemberInfo != null ? o.MemberInfo.Name : "",
                    CreatedDate = o.CreatedDate,              // ← 用 CreatedDate
                    TotalAmount = totals.ContainsKey(o.OrderID) ? totals[o.OrderID] : 0m,
                    PaymentStatus = o.PaymentStatus,
                    ShippingStatus = o.ShippingStatus,
                    OrderStatus = o.OrderStatus
                })
                .ToListAsync();

            ViewBag.Total = total;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Q = q;
            ViewBag.Payment = payment;
            ViewBag.Shipping = shipping;
            ViewBag.Status = status;

            return View(data);
        }

        // ====== Details ======
        public async Task<IActionResult> Details(Guid id)
        {
            var order = await _context.Order
                .Include(o => o.MemberInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null) return NotFound();

            var detailRows = await _context.OrderDetail
                .Where(d => d.OrderID == id)
                .Join(_context.Product,
                      d => d.ProductID,
                      p => p.ProductID,
                      (d, p) => new OrderDetailRowVm
                      {
                          ProductName = p.ProductName,
                          Quantity = d.Quantity,
                          UnitPrice = d.UnitPrice
                      })
                .ToListAsync();

            var vm = new OrderDetailsPageVm
            {
                Order = order,
                Details = detailRows
            };

            return View(vm);
        }

        // ====== Edit (GET) ======
        public async Task<IActionResult> Edit(Guid id)
        {
            var o = await _context.Order
                .Include(x => x.MemberInfo)
                .FirstOrDefaultAsync(x => x.OrderID == id);
            if (o == null) return NotFound();

            var vm = new OrderEditVm
            {
                OrderID = o.OrderID,
                OrderNumber = o.OrderNumber,
                CreatedDate = o.CreatedDate,     // ← 用 CreatedDate
                RecipientName = o.RecipientName,
                RecipientPhone = o.RecipientPhone,
                RecipientAddress = o.RecipientAddress,
                PaymentMethod = o.PaymentMethod,
                PaymentStatus = o.PaymentStatus,
                ShippingStatus = o.ShippingStatus,
                OrderStatus = o.OrderStatus
            };
            return View(vm);
        }

        // ====== Edit (POST) ======
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, OrderEditVm vm)
        {
            if (id != vm.OrderID) return NotFound();
            if (!ModelState.IsValid) return View(vm);

            var o = await _context.Order.FirstOrDefaultAsync(x => x.OrderID == id);
            if (o == null) return NotFound();

            o.RecipientName = vm.RecipientName.Trim();
            o.RecipientPhone = vm.RecipientPhone.Trim();
            o.RecipientAddress = vm.RecipientAddress.Trim();
            o.PaymentMethod = vm.PaymentMethod?.Trim();
            o.PaymentStatus = vm.PaymentStatus;
            o.ShippingStatus = vm.ShippingStatus;
            o.OrderStatus = vm.OrderStatus;
            o.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            TempData["ok"] = "訂單已更新";
            return RedirectToAction(nameof(Index));
        }

        // ====== Delete (GET) ======
        public async Task<IActionResult> Delete(Guid id)
        {
            var o = await _context.Order
                .Include(x => x.MemberInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.OrderID == id);
            if (o == null) return NotFound();
            return View(o);
        }

        // ====== Delete (POST) ======
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var order = await _context.Order.FirstOrDefaultAsync(o => o.OrderID == id);
            if (order == null) return RedirectToAction(nameof(Index));

            // 先刪明細，避免 FK 限制
            var details = _context.OrderDetail.Where(d => d.OrderID == id);
            _context.OrderDetail.RemoveRange(details);

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            TempData["ok"] = "訂單已刪除";
            return RedirectToAction(nameof(Index));
        }
    }
}
