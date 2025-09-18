using FinalExamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly GoShopContext _context;

        public InvoiceController(GoShopContext context)
        {
            _context = context;
        }

        // GET: Invoice
        public async Task<IActionResult> Index([FromQuery] InvoiceQueryVm query)
        {
            var q = _context.Invoice
                .Include(i => i.OrderInfo)
                .AsNoTracking()
                .AsQueryable();

            if (query.OrderID.HasValue)
                q = q.Where(i => i.OrderID == query.OrderID.Value);

            if (!string.IsNullOrWhiteSpace(query.InvoiceNumber))
                q = q.Where(i => i.InvoiceNumber.Contains(query.InvoiceNumber));

            var items = await q.ToListAsync();

            ViewBag.Orders = await _context.Order.AsNoTracking().ToListAsync();
            ViewBag.Query = query;

            return View(items);
        }

        // GET: Invoice/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var invoice = await _context.Invoice
                .Include(i => i.OrderInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.InvoiceID == id);

            if (invoice == null) return NotFound();

            return View(invoice);
        }

        // GET: Invoice/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Orders = await _context.Order.AsNoTracking().ToListAsync();
            return View(new InvoiceCreateVm());
        }

        // POST: Invoice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InvoiceCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Orders = await _context.Order.AsNoTracking().ToListAsync();
                return View(vm);
            }

            var entity = new Invoice
            {
                InvoiceID = Guid.NewGuid(),
                OrderID = vm.OrderID,
                InvoiceNumber = vm.InvoiceNumber.Trim(),
                InvoiceDate = vm.InvoiceDate,
                Amount = vm.Amount,
                Tax = vm.Tax,
                TotalAmount = vm.TotalAmount,
                CreatedDate = DateTime.Now
            };

            _context.Add(entity);
            await _context.SaveChangesAsync();
            TempData["ok"] = "發票建立成功";
            return RedirectToAction(nameof(Index));
        }

        // GET: Invoice/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var invoice = await _context.Invoice.FindAsync(id);
            if (invoice == null) return NotFound();

            var vm = new InvoiceEditVm
            {
                InvoiceID = invoice.InvoiceID,
                OrderID = invoice.OrderID,
                InvoiceNumber = invoice.InvoiceNumber,
                InvoiceDate = invoice.InvoiceDate,
                Amount = invoice.Amount,
                Tax = invoice.Tax,
                TotalAmount = invoice.TotalAmount
            };

            ViewBag.Orders = await _context.Order.AsNoTracking().ToListAsync();
            return View(vm);
        }

        // POST: Invoice/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, InvoiceEditVm vm)
        {
            if (id != vm.InvoiceID) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.Orders = await _context.Order.AsNoTracking().ToListAsync();
                return View(vm);
            }

            var invoice = await _context.Invoice.FirstOrDefaultAsync(i => i.InvoiceID == id);
            if (invoice == null) return NotFound();

            invoice.OrderID = vm.OrderID;
            invoice.InvoiceNumber = vm.InvoiceNumber.Trim();
            invoice.InvoiceDate = vm.InvoiceDate;
            invoice.Amount = vm.Amount;
            invoice.Tax = vm.Tax;
            invoice.TotalAmount = vm.TotalAmount;

            try
            {
                await _context.SaveChangesAsync();
                TempData["ok"] = "發票已更新";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Invoice.Any(e => e.InvoiceID == vm.InvoiceID))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Invoice/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var invoice = await _context.Invoice
                .Include(i => i.OrderInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.InvoiceID == id);

            if (invoice == null) return NotFound();

            return View(invoice);
        }

        // POST: Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var invoice = await _context.Invoice.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoice.Remove(invoice);
                await _context.SaveChangesAsync();
                TempData["ok"] = "發票已刪除";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
