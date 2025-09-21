using FinalExamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Areas.Store.Controllers
{
    [Area("Store")]
    public class CatalogController : Controller
    {
        private readonly GoShopContext _context;

        public CatalogController(GoShopContext context)
        {
            _context = context;
        }

        // GET: /Store/Catalog/Index
        public async Task<IActionResult> Index(string? q, int? categoryId, int page = 1, int pageSize = 12)
        {
            var query = _context.Product
                .Include(p => p.CategoryInfo)
                .AsNoTracking()
                .Where(p => p.IsActive); // 只顯示上架商品

            if (!string.IsNullOrWhiteSpace(q))
            {
                var key = q.Trim();
                query = query.Where(p =>
                    p.ProductName.Contains(key) ||
                    p.ProductCode.Contains(key) ||
                    p.Description.Contains(key));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryID == categoryId.Value);
            }

            // 排序：最新在前
            query = query.OrderByDescending(p => p.CreatedDate);

            var total = await query.CountAsync();
            pageSize = Math.Clamp(pageSize, 6, 48);
            var totalPages = (int)Math.Ceiling(total / (double)pageSize);

            // 讓 page 落在 1..totalPages（total=0 時 totalPages=0，page 固定為 1）
            page = Math.Max(1, page);
            if (totalPages > 0) page = Math.Min(page, totalPages);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Categories = await _context.Category.AsNoTracking().ToListAsync();
            ViewBag.Q = q;
            ViewBag.CategoryId = categoryId;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Total = total;
            ViewBag.TotalPages = totalPages;

            return View(items);
        }

        // GET: /Store/Catalog/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            var item = await _context.Product
                .Include(p => p.CategoryInfo)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductID == id && p.IsActive);

            if (item == null) return NotFound();

            return View(item);
        }
    }
}
