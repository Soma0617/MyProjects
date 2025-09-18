using FinalExamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Controllers
{
    public class InventoryController : Controller
    {
        private readonly GoShopContext _context;

        public InventoryController(GoShopContext context)
        {
            _context = context;
        }

        // GET: Inventory
        public async Task<IActionResult> Index([FromQuery] InventoryQueryVm query)
        {
            var q = _context.Inventory
                .Include(i => i.ProductInfo)
                .AsNoTracking()
                .AsQueryable();

            if (query.ProductID.HasValue)
                q = q.Where(i => i.ProductID == query.ProductID.Value);

            if (query.MinQty.HasValue)
                q = q.Where(i => i.Quantity >= query.MinQty.Value);

            if (query.MaxQty.HasValue)
                q = q.Where(i => i.Quantity <= query.MaxQty.Value);

            var items = await q.ToListAsync();

            ViewBag.Products = await _context.Product.AsNoTracking().ToListAsync();
            ViewBag.Query = query;

            return View(items);
        }

        // GET: Inventory/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var inventory = await _context.Inventory
                .Include(i => i.ProductInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.InventoryID == id);

            if (inventory == null) return NotFound();

            return View(inventory);
        }

        // GET: Inventory/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Products = await _context.Product.AsNoTracking().ToListAsync();
            return View(new InventoryCreateVm());
        }

        // POST: Inventory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InventoryCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = await _context.Product.AsNoTracking().ToListAsync();
                return View(vm);
            }

            var entity = new Inventory
            {
                InventoryID = Guid.NewGuid(),
                ProductID = vm.ProductID,
                Quantity = vm.Quantity,
                SafetyStock = vm.SafetyStock,
                LastUpdated = DateTime.Now
            };

            _context.Add(entity);
            await _context.SaveChangesAsync();
            TempData["ok"] = "庫存新增成功";
            return RedirectToAction(nameof(Index));
        }

        // GET: Inventory/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory == null) return NotFound();

            var vm = new InventoryEditVm
            {
                InventoryID = inventory.InventoryID,
                ProductID = inventory.ProductID,
                Quantity = inventory.Quantity,
                SafetyStock = inventory.SafetyStock
            };

            ViewBag.Products = await _context.Product.AsNoTracking().ToListAsync();
            return View(vm);
        }

        // POST: Inventory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, InventoryEditVm vm)
        {
            if (id != vm.InventoryID) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.Products = await _context.Product.AsNoTracking().ToListAsync();
                return View(vm);
            }

            var inventory = await _context.Inventory.FirstOrDefaultAsync(i => i.InventoryID == id);
            if (inventory == null) return NotFound();

            inventory.ProductID = vm.ProductID;
            inventory.Quantity = vm.Quantity;
            inventory.SafetyStock = vm.SafetyStock;
            inventory.LastUpdated = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                TempData["ok"] = "庫存已更新";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Inventory.Any(e => e.InventoryID == vm.InventoryID))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Inventory/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var inventory = await _context.Inventory
                .Include(i => i.ProductInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.InventoryID == id);

            if (inventory == null) return NotFound();

            return View(inventory);
        }

        // POST: Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory != null)
            {
                _context.Inventory.Remove(inventory);
                await _context.SaveChangesAsync();
                TempData["ok"] = "庫存已刪除";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
