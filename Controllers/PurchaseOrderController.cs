using FinalExamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private readonly GoShopContext _context;

        public PurchaseOrderController(GoShopContext context)
        {
            _context = context;
        }

        // GET: PurchaseOrder
        public async Task<IActionResult> Index([FromQuery] PurchaseOrderQueryVm query)
        {
            var q = _context.PurchaseOrder
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SupplierName))
                q = q.Where(po => po.SupplierName.Contains(query.SupplierName));

            if (!string.IsNullOrWhiteSpace(query.Status))
                q = q.Where(po => po.Status == query.Status);

            var items = await q.ToListAsync();
            ViewBag.Query = query;

            return View(items);
        }

        // GET: PurchaseOrder/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var po = await _context.PurchaseOrder
                .Include(p => p.PurchaseOrderDetails)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.PurchaseOrderID == id);

            if (po == null) return NotFound();

            return View(po);
        }

        // GET: PurchaseOrder/Create
        public IActionResult Create()
        {
            return View(new PurchaseOrderCreateVm());
        }

        // POST: PurchaseOrder/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseOrderCreateVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = new PurchaseOrder
            {
                PurchaseOrderID = Guid.NewGuid(),
                PurchaseOrderCode = vm.PurchaseOrderCode.Trim(),
                SupplierName = vm.SupplierName.Trim(),
                Status = vm.Status,
                CreatedDate = DateTime.Now
            };

            _context.Add(entity);
            await _context.SaveChangesAsync();
            TempData["ok"] = "採購單新增成功";
            return RedirectToAction(nameof(Index));
        }

        // GET: PurchaseOrder/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var po = await _context.PurchaseOrder.FindAsync(id);
            if (po == null) return NotFound();

            var vm = new PurchaseOrderEditVm
            {
                PurchaseOrderID = po.PurchaseOrderID,
                PurchaseOrderCode = po.PurchaseOrderCode,
                SupplierName = po.SupplierName,
                Status = po.Status
            };

            return View(vm);
        }

        // POST: PurchaseOrder/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PurchaseOrderEditVm vm)
        {
            if (id != vm.PurchaseOrderID) return NotFound();
            if (!ModelState.IsValid) return View(vm);

            var po = await _context.PurchaseOrder.FirstOrDefaultAsync(p => p.PurchaseOrderID == id);
            if (po == null) return NotFound();

            po.PurchaseOrderCode = vm.PurchaseOrderCode.Trim();
            po.SupplierName = vm.SupplierName.Trim();
            po.Status = vm.Status;
            po.UpdatedDate = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                TempData["ok"] = "採購單已更新";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.PurchaseOrder.Any(e => e.PurchaseOrderID == vm.PurchaseOrderID))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: PurchaseOrder/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var po = await _context.PurchaseOrder
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.PurchaseOrderID == id);

            if (po == null) return NotFound();

            return View(po);
        }

        // POST: PurchaseOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var po = await _context.PurchaseOrder.FindAsync(id);
            if (po != null)
            {
                _context.PurchaseOrder.Remove(po);
                await _context.SaveChangesAsync();
                TempData["ok"] = "採購單已刪除";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
