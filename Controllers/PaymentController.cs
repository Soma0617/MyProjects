using FinalExamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Controllers
{
    public class PaymentController : Controller
    {
        private readonly GoShopContext _context;

        public PaymentController(GoShopContext context)
        {
            _context = context;
        }

        // GET: Payment
        public async Task<IActionResult> Index([FromQuery] PaymentQueryVm query)
        {
            var q = _context.Payment
                .Include(p => p.OrderInfo)
                .AsNoTracking()
                .AsQueryable();

            if (query.OrderID.HasValue)
                q = q.Where(p => p.OrderID == query.OrderID.Value);

            if (!string.IsNullOrWhiteSpace(query.Status))
                q = q.Where(p => p.Status == query.Status);

            if (!string.IsNullOrWhiteSpace(query.PaymentMethod))
                q = q.Where(p => p.PaymentMethod == query.PaymentMethod);

            var items = await q.ToListAsync();

            ViewBag.Orders = await _context.Order.AsNoTracking().ToListAsync();
            ViewBag.Query = query;

            return View(items);
        }

        // GET: Payment/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var payment = await _context.Payment
                .Include(p => p.OrderInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.PaymentID == id);

            if (payment == null) return NotFound();

            return View(payment);
        }

        // GET: Payment/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Orders = await _context.Order.AsNoTracking().ToListAsync();
            return View(new PaymentCreateVm());
        }

        // POST: Payment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Orders = await _context.Order.AsNoTracking().ToListAsync();
                return View(vm);
            }

            var entity = new Payment
            {
                PaymentID = Guid.NewGuid(),
                OrderID = vm.OrderID,
                PaymentMethod = vm.PaymentMethod.Trim(),
                TotalAmount = vm.TotalAmount,
                PaymentDate = vm.PaymentDate,
                Status = vm.Status,
                TransactionID = vm.TransactionID,
                CreatedDate = DateTime.Now
            };

            _context.Add(entity);
            await _context.SaveChangesAsync();
            TempData["ok"] = "付款紀錄新增成功";
            return RedirectToAction(nameof(Index));
        }

        // GET: Payment/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var payment = await _context.Payment.FindAsync(id);
            if (payment == null) return NotFound();

            var vm = new PaymentEditVm
            {
                PaymentID = payment.PaymentID,
                OrderID = payment.OrderID,
                PaymentMethod = payment.PaymentMethod,
                TotalAmount = payment.TotalAmount,
                PaymentDate = payment.PaymentDate,
                Status = payment.Status,
                TransactionID = payment.TransactionID
            };

            ViewBag.Orders = await _context.Order.AsNoTracking().ToListAsync();
            return View(vm);
        }

        // POST: Payment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PaymentEditVm vm)
        {
            if (id != vm.PaymentID) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.Orders = await _context.Order.AsNoTracking().ToListAsync();
                return View(vm);
            }

            var payment = await _context.Payment.FirstOrDefaultAsync(p => p.PaymentID == id);
            if (payment == null) return NotFound();

            payment.OrderID = vm.OrderID;
            payment.PaymentMethod = vm.PaymentMethod.Trim();
            payment.TotalAmount = vm.TotalAmount;
            payment.PaymentDate = vm.PaymentDate;
            payment.Status = vm.Status;
            payment.TransactionID = vm.TransactionID;

            try
            {
                await _context.SaveChangesAsync();
                TempData["ok"] = "付款紀錄已更新";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Payment.Any(e => e.PaymentID == vm.PaymentID))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Payment/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var payment = await _context.Payment
                .Include(p => p.OrderInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.PaymentID == id);

            if (payment == null) return NotFound();

            return View(payment);
        }

        // POST: Payment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var payment = await _context.Payment.FindAsync(id);
            if (payment != null)
            {
                _context.Payment.Remove(payment);
                await _context.SaveChangesAsync();
                TempData["ok"] = "付款紀錄已刪除";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
