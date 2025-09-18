using FinalExamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Controllers
{
    public class OrderDetailController : Controller
    {
        private readonly GoShopContext _context;

        public OrderDetailController(GoShopContext context)
        {
            _context = context;
        }

        // GET: OrderDetail
        public async Task<IActionResult> Index([FromQuery] OrderDetailQueryVm query)
        {
            var q = _context.OrderDetail
                .Include(o => o.OrderInfo)
                .Include(o => o.ProductInfo)
                .AsNoTracking()
                .AsQueryable();

            if (query.OrderID.HasValue)
                q = q.Where(od => od.OrderID == query.OrderID.Value);

            if (query.ProductID.HasValue)
                q = q.Where(od => od.ProductID == query.ProductID.Value);

            var items = await q.ToListAsync();

            ViewBag.Orders = await _context.Order.AsNoTracking().ToListAsync();
            ViewBag.Products = await _context.Product.AsNoTracking().ToListAsync();
            ViewBag.Query = query;

            return View(items);
        }

        // GET: OrderDetail/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var od = await _context.OrderDetail
                .Include(o => o.OrderInfo)
                .Include(o => o.ProductInfo)
                .FirstOrDefaultAsync(m => m.OrderDetailID == id);

            if (od == null) return NotFound();

            return View(od);
        }

        // GET: OrderDetail/Create
        public IActionResult Create()
        {
            ViewBag.Orders = _context.Order.AsNoTracking().ToList();
            ViewBag.Products = _context.Product.AsNoTracking().ToList();
            return View(new OrderDetailCreateVm());
        }

        // POST: OrderDetail/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderDetailCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Orders = _context.Order.AsNoTracking().ToList();
                ViewBag.Products = _context.Product.AsNoTracking().ToList();
                return View(vm);
            }

            var entity = new OrderDetail
            {
                OrderDetailID = Guid.NewGuid(),
                OrderID = vm.OrderID,
                ProductID = vm.ProductID,
                Quantity = vm.Quantity,
                UnitPrice = vm.UnitPrice
            };

            _context.OrderDetail.Add(entity);
            await _context.SaveChangesAsync();

            TempData["ok"] = "新增明細成功";
            return RedirectToAction(nameof(Index));
        }

        // GET: OrderDetail/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var od = await _context.OrderDetail.FindAsync(id);
            if (od == null) return NotFound();

            var vm = new OrderDetailEditVm
            {
                OrderDetailID = od.OrderDetailID,
                OrderID = od.OrderID,
                ProductID = od.ProductID,
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice
            };

            ViewBag.Orders = _context.Order.AsNoTracking().ToList();
            ViewBag.Products = _context.Product.AsNoTracking().ToList();

            return View(vm);
        }

        // POST: OrderDetail/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, OrderDetailEditVm vm)
        {
            if (id != vm.OrderDetailID) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.Orders = _context.Order.AsNoTracking().ToList();
                ViewBag.Products = _context.Product.AsNoTracking().ToList();
                return View(vm);
            }

            var od = await _context.OrderDetail.FirstOrDefaultAsync(x => x.OrderDetailID == id);
            if (od == null) return NotFound();

            od.OrderID = vm.OrderID;
            od.ProductID = vm.ProductID;
            od.Quantity = vm.Quantity;
            od.UnitPrice = vm.UnitPrice;

            await _context.SaveChangesAsync();
            TempData["ok"] = "明細已更新";

            return RedirectToAction(nameof(Index));
        }

        // GET: OrderDetail/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var od = await _context.OrderDetail
                .Include(o => o.OrderInfo)
                .Include(o => o.ProductInfo)
                .FirstOrDefaultAsync(m => m.OrderDetailID == id);

            if (od == null) return NotFound();

            return View(od);
        }

        // POST: OrderDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var od = await _context.OrderDetail.FindAsync(id);
            if (od != null)
            {
                _context.OrderDetail.Remove(od);
                await _context.SaveChangesAsync();
            }

            TempData["ok"] = "明細已刪除";
            return RedirectToAction(nameof(Index));
        }
    }
}
