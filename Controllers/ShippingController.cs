using FinalExamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Controllers
{
    public class ShippingController : Controller
    {
        private readonly GoShopContext _context;

        public ShippingController(GoShopContext context)
        {
            _context = context;
        }

        // GET: Shipping
        public async Task<IActionResult> Index([FromQuery] ShippingQueryVm query)
        {
            var q = _context.Shipping
                .Include(s => s.OrderInfo)
                .AsNoTracking()
                .AsQueryable();

            if (query.OrderID.HasValue)
                q = q.Where(s => s.OrderID == query.OrderID.Value);

            if (!string.IsNullOrWhiteSpace(query.Status))
                q = q.Where(s => s.Status == query.Status);

            var items = await q.ToListAsync();

            ViewBag.Orders = await _context.Order.AsNoTracking().ToListAsync();
            ViewBag.Query = query;

            return View(items);
        }

        // GET: Shipping/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var shipping = await _context.Shipping
                .Include(s => s.OrderInfo)
                .FirstOrDefaultAsync(m => m.ShippingID == id);

            if (shipping == null) return NotFound();

            return View(shipping);
        }

        // GET: Shipping/Create
        public IActionResult Create()
        {
            ViewBag.Orders = _context.Order.AsNoTracking().ToList();
            return View(new ShippingCreateVm());
        }

        // POST: Shipping/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShippingCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Orders = _context.Order.AsNoTracking().ToList();
                return View(vm);
            }

            var entity = new Shipping
            {
                ShippingID = Guid.NewGuid(),
                OrderID = vm.OrderID,
                TrackingNumber = vm.TrackingNumber,
                Carrier = vm.Carrier,
                RecipientName = vm.RecipientName,
                ShippingAddress = vm.ShippingAddress,
                RecipientPhone = vm.RecipientPhone,
                ShippedDate = vm.ShippedDate,
                EstimatedArrivalDate = vm.EstimatedArrivalDate,
                DeliveredDate = vm.DeliveredDate,
                Status = vm.Status,
                CreatedDate = DateTime.Now
            };

            _context.Shipping.Add(entity);
            await _context.SaveChangesAsync();

            TempData["ok"] = "新增出貨紀錄成功";
            return RedirectToAction(nameof(Index));
        }

        // GET: Shipping/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var shipping = await _context.Shipping.FindAsync(id);
            if (shipping == null) return NotFound();

            var vm = new ShippingEditVm
            {
                ShippingID = shipping.ShippingID,
                OrderID = shipping.OrderID,
                TrackingNumber = shipping.TrackingNumber,
                Carrier = shipping.Carrier,
                RecipientName = shipping.RecipientName,
                ShippingAddress = shipping.ShippingAddress,
                RecipientPhone = shipping.RecipientPhone,
                ShippedDate = shipping.ShippedDate,
                EstimatedArrivalDate = shipping.EstimatedArrivalDate,
                DeliveredDate = shipping.DeliveredDate,
                Status = shipping.Status
            };

            ViewBag.Orders = _context.Order.AsNoTracking().ToList();
            return View(vm);
        }

        // POST: Shipping/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ShippingEditVm vm)
        {
            if (id != vm.ShippingID) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.Orders = _context.Order.AsNoTracking().ToList();
                return View(vm);
            }

            var shipping = await _context.Shipping.FirstOrDefaultAsync(s => s.ShippingID == id);
            if (shipping == null) return NotFound();

            shipping.OrderID = vm.OrderID;
            shipping.TrackingNumber = vm.TrackingNumber;
            shipping.Carrier = vm.Carrier;
            shipping.RecipientName = vm.RecipientName;
            shipping.ShippingAddress = vm.ShippingAddress;
            shipping.RecipientPhone = vm.RecipientPhone;
            shipping.ShippedDate = vm.ShippedDate;
            shipping.EstimatedArrivalDate = vm.EstimatedArrivalDate;
            shipping.DeliveredDate = vm.DeliveredDate;
            shipping.Status = vm.Status;

            await _context.SaveChangesAsync();
            TempData["ok"] = "出貨紀錄已更新";

            return RedirectToAction(nameof(Index));
        }

        // GET: Shipping/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var shipping = await _context.Shipping
                .Include(s => s.OrderInfo)
                .FirstOrDefaultAsync(m => m.ShippingID == id);

            if (shipping == null) return NotFound();

            return View(shipping);
        }

        // POST: Shipping/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var shipping = await _context.Shipping.FindAsync(id);
            if (shipping != null)
            {
                _context.Shipping.Remove(shipping);
                await _context.SaveChangesAsync();
            }

            TempData["ok"] = "出貨紀錄已刪除";
            return RedirectToAction(nameof(Index));
        }
    }
}
