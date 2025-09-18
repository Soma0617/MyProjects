using FinalExamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly GoShopContext _context;

        public ProductController(GoShopContext context)
        {
            _context = context;
        }

        // GET: Product/Index
        public async Task<IActionResult> Index([FromQuery] ProductQueryVm query)
        {
            var q = _context.Product
                .Include(p => p.CategoryInfo)
                .AsNoTracking();

            // 關鍵字
            if (!string.IsNullOrWhiteSpace(query.Q))
            {
                var key = query.Q.Trim();
                q = q.Where(p =>
                    p.ProductName.Contains(key) ||
                    p.ProductCode.Contains(key) ||
                    p.Description.Contains(key));
            }

            // 分類
            if (query.CategoryID.HasValue)
            {
                q = q.Where(p => p.CategoryID == query.CategoryID.Value);
            }

            // 是否上架
            if (query.IsActive.HasValue)
            {
                q = q.Where(p => p.IsActive == query.IsActive.Value);
            }

            // 排序
            q = query.Sort switch
            {
                "name" => q.OrderBy(p => p.ProductName),
                "name_desc" => q.OrderByDescending(p => p.ProductName),
                "price" => q.OrderBy(p => p.Price),
                "price_desc" => q.OrderByDescending(p => p.Price),
                "created" => q.OrderBy(p => p.CreatedDate),
                _ => q.OrderByDescending(p => p.CreatedDate) // 預設
            };

            // 分頁
            var total = await q.CountAsync();
            var pageSize = Math.Clamp(query.PageSize, 5, 50);
            var page = Math.Max(1, query.Page);

            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewBag.Total = total;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Sort = query.Sort;
            ViewBag.Query = query;
            ViewBag.Categories = await _context.Category.AsNoTracking().ToListAsync();

            return View(items);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Product
                .Include(p => p.CategoryInfo)
                .FirstOrDefaultAsync(m => m.ProductID == id);

            if (product == null) return NotFound();

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Category.AsNoTracking().ToList();
            return View(new ProductCreateVm());
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Category.AsNoTracking().ToList();
                return View(vm);
            }

            var entity = new Product
            {
                ProductID = Guid.NewGuid(),
                ProductCode = vm.ProductCode,
                ProductName = vm.ProductName,
                Description = vm.Description,
                Photo = vm.Photo,
                Price = vm.Price,
                IsActive = vm.IsActive,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CategoryID = vm.CategoryID
            };

            _context.Product.Add(entity);
            await _context.SaveChangesAsync();

            TempData["ok"] = "新增商品成功";
            return RedirectToAction(nameof(Index));
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Product.FindAsync(id);
            if (product == null) return NotFound();

            var vm = new ProductEditVm
            {
                ProductID = product.ProductID,
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                Description = product.Description,
                Photo = product.Photo,
                Price = product.Price,
                IsActive = product.IsActive,
                CategoryID = product.CategoryID
            };

            ViewBag.Categories = _context.Category.AsNoTracking().ToList();
            return View(vm);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductEditVm vm)
        {
            if (id != vm.ProductID) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Category.AsNoTracking().ToList();
                return View(vm);
            }

            var product = await _context.Product.FirstOrDefaultAsync(p => p.ProductID == id);
            if (product == null) return NotFound();

            product.ProductCode = vm.ProductCode;
            product.ProductName = vm.ProductName;
            product.Description = vm.Description;
            product.Photo = vm.Photo;
            product.Price = vm.Price;
            product.IsActive = vm.IsActive;
            product.UpdatedDate = DateTime.Now;
            product.CategoryID = vm.CategoryID;

            await _context.SaveChangesAsync();
            TempData["ok"] = "修改成功";

            return RedirectToAction(nameof(Index));
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Product
                .Include(p => p.CategoryInfo)
                .FirstOrDefaultAsync(m => m.ProductID == id);

            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
            }

            TempData["ok"] = "刪除成功";
            return RedirectToAction(nameof(Index));
        }
    }
}
