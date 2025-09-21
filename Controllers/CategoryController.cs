using FinalExamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly GoShopContext _context;

        public CategoryController(GoShopContext context)
        {
            _context = context;
        }

        // GET: Category
        public async Task<IActionResult> Index([FromQuery] CategoryQueryVm query)
        {
            var q = _context.Category.AsNoTracking().AsQueryable();

            // 關鍵字搜尋（名稱/代碼）
            if (!string.IsNullOrWhiteSpace(query.Q))
            {
                var key = query.Q.Trim();
                q = q.Where(c => c.CategoryName.Contains(key) || c.CategoryCode.Contains(key));
            }

            // 排序
            q = query.Sort switch
            {
                "code" => q.OrderBy(c => c.CategoryCode),
                "code_desc" => q.OrderByDescending(c => c.CategoryCode),
                "name_desc" => q.OrderByDescending(c => c.CategoryName),
                _ => q.OrderBy(c => c.CategoryName),
            };

            // 分頁
            var total = await q.CountAsync();
            var pageSize = Math.Clamp(query.PageSize, 5, 50);
            var page = Math.Max(1, query.Page);
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewBag.Total = total;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Query = query;

            return View(items);
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Category
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CategoryID == id);

            if (category == null) return NotFound();

            return View(category);
        }

        // GET: Category/Create
        public IActionResult Create() => View(new CategoryCreateVm());

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            vm.CategoryName = vm.CategoryName?.Trim() ?? string.Empty;
            vm.CategoryCode = vm.CategoryCode?.Trim() ?? string.Empty;

            // 唯一性：代碼不可重複
            var codeExists = await _context.Category.AnyAsync(c => c.CategoryCode == vm.CategoryCode);
            if (codeExists)
            {
                ModelState.AddModelError(nameof(vm.CategoryCode), "分類代碼已存在");
                return View(vm);
            }

            var entity = new Category
            {
                CategoryName = vm.CategoryName,
                CategoryCode = vm.CategoryCode
            };

            _context.Add(entity);
            await _context.SaveChangesAsync();

            TempData["ok"] = "分類建立成功";
            return RedirectToAction(nameof(Index));
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Category.FindAsync(id);
            if (category == null) return NotFound();

            var vm = new CategoryEditVm
            {
                CategoryID = category.CategoryID,
                CategoryName = category.CategoryName,
                CategoryCode = category.CategoryCode
            };

            return View(vm);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryEditVm vm)
        {
            if (id != vm.CategoryID) return NotFound();
            if (!ModelState.IsValid) return View(vm);

            var category = await _context.Category.FindAsync(id);
            if (category == null) return NotFound();

            vm.CategoryName = vm.CategoryName?.Trim() ?? string.Empty;
            vm.CategoryCode = vm.CategoryCode?.Trim() ?? string.Empty;

            // 唯一性：代碼不可與其他分類重複
            var codeExists = await _context.Category
                .AnyAsync(c => c.CategoryCode == vm.CategoryCode && c.CategoryID != id);
            if (codeExists)
            {
                ModelState.AddModelError(nameof(vm.CategoryCode), "分類代碼已存在");
                return View(vm);
            }

            category.CategoryName = vm.CategoryName;
            category.CategoryCode = vm.CategoryCode;

            await _context.SaveChangesAsync();
            TempData["ok"] = "分類已更新";

            return RedirectToAction(nameof(Index));
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Category
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CategoryID == id);

            if (category == null) return NotFound();

            ViewBag.InUse = await _context.Product.AnyAsync(p => p.CategoryID == id);
            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // 有商品使用就擋下
            var inUse = await _context.Product.AnyAsync(p => p.CategoryID == id);
            if (inUse)
            {
                TempData["err"] = "此分類已有商品使用，無法刪除。請先調整商品分類。";
                return RedirectToAction(nameof(Delete), new { id });
            }

            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                _context.Category.Remove(category);
                await _context.SaveChangesAsync();
                TempData["ok"] = "分類已刪除";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
