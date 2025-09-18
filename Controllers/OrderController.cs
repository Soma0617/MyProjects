using FinalExamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Controllers
{
    public class OrderController : Controller
    {
        private readonly GoShopContext _context;

        public OrderController(GoShopContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Order
                .Include(o => o.MemberInfo)   // 如果你有設 Member 關聯
                .AsNoTracking()
                .ToListAsync();

            return View(orders);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            // 會員下拉
            ViewBag.Members = _context.Member.AsNoTracking().ToList();

            // 預設表單值
            var vm = new OrderCreateVm
            {
                OrderNumber = DateTime.Now.ToString("yyyyMMddHHmm"),
                PaymentStatus = "Unpaid",
                ShippingStatus = "Pending",
                OrderStatus = "Active"
            };
            return View(vm);
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Members = _context.Member.AsNoTracking().ToList();
                return View(vm);
            }

            var entity = new Order
            {
                OrderID = Guid.NewGuid(),
                OrderNumber = vm.OrderNumber,
                MemberID = vm.MemberID,
                RecipientName = vm.RecipientName,
                RecipientPhone = vm.RecipientPhone,
                RecipientAddress = vm.RecipientAddress,
                PaymentStatus = vm.PaymentStatus,
                PaymentMethod = vm.PaymentMethod,
                ShippingStatus = vm.ShippingStatus,
                OrderStatus = vm.OrderStatus,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            _context.Order.Add(entity);
            await _context.SaveChangesAsync();
            TempData["ok"] = "訂單建立成功";
            return RedirectToAction(nameof(Index));
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Order.AsNoTracking().FirstOrDefaultAsync(o => o.OrderID == id);
            if (order == null) return NotFound();

            var vm = new OrderEditVm
            {
                OrderID = order.OrderID,
                OrderNumber = order.OrderNumber,
                MemberID = order.MemberID,
                RecipientName = order.RecipientName,
                RecipientPhone = order.RecipientPhone,
                RecipientAddress = order.RecipientAddress,
                PaymentStatus = order.PaymentStatus,
                PaymentMethod = order.PaymentMethod,
                ShippingStatus = order.ShippingStatus,
                OrderStatus = order.OrderStatus
            };

            ViewBag.Members = _context.Member.AsNoTracking().ToList();
            return View(vm);
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, OrderEditVm vm)
        {
            if (id != vm.OrderID) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Members = _context.Member.AsNoTracking().ToList();
                return View(vm);
            }

            var order = await _context.Order.FirstOrDefaultAsync(o => o.OrderID == id);
            if (order == null) return NotFound();

            // 更新欄位
            order.OrderNumber = vm.OrderNumber.Trim();
            order.MemberID = vm.MemberID;
            order.RecipientName = vm.RecipientName.Trim();
            order.RecipientPhone = vm.RecipientPhone.Trim();
            order.RecipientAddress = vm.RecipientAddress.Trim();
            order.PaymentStatus = vm.PaymentStatus.Trim();
            order.PaymentMethod = string.IsNullOrWhiteSpace(vm.PaymentMethod) ? null : vm.PaymentMethod.Trim();
            order.ShippingStatus = vm.ShippingStatus.Trim();
            order.OrderStatus = vm.OrderStatus.Trim();
            order.UpdatedDate = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                TempData["ok"] = "訂單已更新";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Order.AnyAsync(e => e.OrderID == id)) return NotFound();
                throw;
            }
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Order
                .Include(o => o.MemberInfo)   // 顯示用
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
                await _context.SaveChangesAsync();
                TempData["ok"] = "訂單已刪除";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Order
                .Include(o => o.MemberInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null) return NotFound();

            return View(order);
        }
    }
}
