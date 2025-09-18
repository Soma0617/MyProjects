using FinalExamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Controllers
{
    public class PurchaseOrderDetailController : Controller
    {
        private readonly GoShopContext _context;

        public PurchaseOrderDetailController(GoShopContext context)
        {
            _context = context;
        }

        // GET: PurchaseOrderDetail
        public async Task<IActionResult> Index([FromQuery] PurchaseOrderDetailQueryVm query)
        {
            var q = _context.PurchaseOrderDetail
                .Include(d => d.PurchaseOrderInfo)
                .Include(d => d.ProductInfo)
                .AsNoTracking()
                .AsQueryable();

            if (query.PurchaseOrderID.HasValue)
                q = q.Where(d => d.PurchaseOrderID == query.PurchaseOrderID.Value);

            if (query.ProductID.HasValue)
                q = q.Where(d => d.ProductID == query.ProductID.Value);

            var items = await q.ToListAsync();

            ViewBag.PurchaseOrders = await _context.PurchaseOrder.AsNoTracking().ToListAsync();
            ViewBag.Products = await _context.Product.AsNoTracking().ToListAsync();
            ViewBag.Query = query;

            return View(items);
        }

        // GET: PurchaseOrderDetail/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var detail = await _context.PurchaseOrderDetail
                .Include(d => d.PurchaseOrderInfo)
                .Include(d => d.ProductInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.PurchaseOrderDetailID == id);

            if (detail == null) return NotFound();

            return View(detail);
        }

        // GET: PurchaseOrderDetail/Create
        public async Task<IActionResult> Create(Guid? purchaseOrderId)
        {
            ViewBag.PurchaseOrders = await _context.PurchaseOrder.AsNoTracking().ToListAsync();
            ViewBag.Products = await _context.Product.AsNoTracking().ToListAsync();

            // 若從採購單頁面點進來，可預選
            return View(new PurchaseOrderDetailCreateVm
            {
                PurchaseOrderID = purchaseOrderId ?? Guid.Empty
            });
        }

        // POST: PurchaseOrderDetail/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseOrderDetailCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PurchaseOrders = await _context.PurchaseOrder.AsNoTracking().ToListAsync();
                ViewBag.Products = await _context.Product.AsNoTracking().ToListAsync();
                return View(vm);
            }

            var entity = new PurchaseOrderDetail
            {
                PurchaseOrderDetailID = Guid.NewGuid(),
                PurchaseOrderID = vm.PurchaseOrderID,
                ProductID = vm.ProductID,
                Quantity = vm.Quantity,
                UnitPrice = vm.UnitPrice
            };

            _context.Add(entity);
            await _context.SaveChangesAsync();
            TempData["ok"] = "採購單明細新增成功";
            return RedirectToAction(nameof(Index), new { PurchaseOrderID = vm.PurchaseOrderID });
        }

        // GET: PurchaseOrderDetail/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var detail = await _context.PurchaseOrderDetail.FindAsync(id);
            if (detail == null) return NotFound();

            var vm = new PurchaseOrderDetailEditVm
            {
                PurchaseOrderDetailID = detail.PurchaseOrderDetailID,
                PurchaseOrderID = detail.PurchaseOrderID,
                ProductID = detail.ProductID,
                Quantity = detail.Quantity,
                UnitPrice = detail.UnitPrice
            };

            ViewBag.PurchaseOrders = await _context.PurchaseOrder.AsNoTracking().ToListAsync();
            ViewBag.Products = await _context.Product.AsNoTracking().ToListAsync();

            return View(vm);
        }

        // POST: PurchaseOrderDetail/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PurchaseOrderDetailEditVm vm)
        {
            if (id != vm.PurchaseOrderDetailID) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.PurchaseOrders = await _context.PurchaseOrder.AsNoTracking().ToListAsync();
                ViewBag.Products = await _context.Product.AsNoTracking().ToListAsync();
                return View(vm);
            }

            var detail = await _context.PurchaseOrderDetail.FirstOrDefaultAsync(d => d.PurchaseOrderDetailID == id);
            if (detail == null) return NotFound();

            detail.PurchaseOrderID = vm.PurchaseOrderID;
            detail.ProductID = vm.ProductID;
            detail.Quantity = vm.Quantity;
            detail.UnitPrice = vm.UnitPrice;

            try
            {
                await _context.SaveChangesAsync();
                TempData["ok"] = "採購單明細已更新";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.PurchaseOrderDetail.Any(e => e.PurchaseOrderDetailID == vm.PurchaseOrderDetailID))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index), new { PurchaseOrderID = vm.PurchaseOrderID });
        }

        // GET: PurchaseOrderDetail/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var detail = await _context.PurchaseOrderDetail
                .Include(d => d.PurchaseOrderInfo)
                .Include(d => d.ProductInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.PurchaseOrderDetailID == id);

            if (detail == null) return NotFound();

            return View(detail);
        }

        // POST: PurchaseOrderDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var detail = await _context.PurchaseOrderDetail.FindAsync(id);
            if (detail != null)
            {
                var poid = detail.PurchaseOrderID;
                _context.PurchaseOrderDetail.Remove(detail);
                await _context.SaveChangesAsync();
                TempData["ok"] = "採購單明細已刪除";
                return RedirectToAction(nameof(Index), new { PurchaseOrderID = poid });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
