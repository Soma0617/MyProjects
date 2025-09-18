using FinalExamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Controllers
{
    public class ReportController : Controller
    {
        private readonly GoShopContext _context;

        public ReportController(GoShopContext context)
        {
            _context = context;
        }

        // GET: Report
        public async Task<IActionResult> Index([FromQuery] ReportQueryVm query)
        {
            var q = _context.Report.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.ReportName))
                q = q.Where(r => r.ReportName.Contains(query.ReportName));

            if (!string.IsNullOrWhiteSpace(query.ReportType))
                q = q.Where(r => r.ReportType == query.ReportType);

            var items = await q.OrderByDescending(r => r.CreatedDate).ToListAsync();

            ViewBag.Query = query;
            return View(items);
        }

        // GET: Report/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var report = await _context.Report.AsNoTracking()
                .FirstOrDefaultAsync(m => m.ReportID == id);
            if (report == null) return NotFound();

            return View(report);
        }

        // GET: Report/Create
        public IActionResult Create()
        {
            return View(new ReportCreateVm());
        }

        // POST: Report/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReportCreateVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = new Report
            {
                ReportID = Guid.NewGuid(),
                ReportName = vm.ReportName.Trim(),
                ReportType = vm.ReportType.Trim(),
                Content = vm.Content.Trim(),
                CreatedBy = vm.CreatedBy.Trim(),
                CreatedDate = DateTime.Now
            };

            _context.Add(entity);
            await _context.SaveChangesAsync();
            TempData["ok"] = "報表建立成功";
            return RedirectToAction(nameof(Index));
        }

        // GET: Report/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var report = await _context.Report.FindAsync(id);
            if (report == null) return NotFound();

            var vm = new ReportEditVm
            {
                ReportID = report.ReportID,
                ReportName = report.ReportName,
                ReportType = report.ReportType,
                Content = report.Content,
                CreatedBy = report.CreatedBy
            };

            return View(vm);
        }

        // POST: Report/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ReportEditVm vm)
        {
            if (id != vm.ReportID) return NotFound();
            if (!ModelState.IsValid) return View(vm);

            var report = await _context.Report.FirstOrDefaultAsync(r => r.ReportID == id);
            if (report == null) return NotFound();

            report.ReportName = vm.ReportName.Trim();
            report.ReportType = vm.ReportType.Trim();
            report.Content = vm.Content.Trim();
            report.CreatedBy = vm.CreatedBy.Trim();

            await _context.SaveChangesAsync();
            TempData["ok"] = "報表更新成功";
            return RedirectToAction(nameof(Index));
        }

        // GET: Report/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var report = await _context.Report.AsNoTracking()
                .FirstOrDefaultAsync(r => r.ReportID == id);
            if (report == null) return NotFound();

            return View(report);
        }

        // POST: Report/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var report = await _context.Report.FindAsync(id);
            if (report != null)
            {
                _context.Report.Remove(report);
                await _context.SaveChangesAsync();
                TempData["ok"] = "報表刪除成功";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
