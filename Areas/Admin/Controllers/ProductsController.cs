using FinalExamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly GoShopContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductsController(GoShopContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // 共用：讀分類下拉
        private async Task SetCategoriesAsync(object? selected = null)
        {
            var list = await _context.Category
                                     .AsNoTracking()
                                     .OrderBy(c => c.CategoryName)
                                     .Select(c => new { c.CategoryID, c.CategoryName })
                                     .ToListAsync();
            ViewBag.Categories = new SelectList(list, "CategoryID", "CategoryName", selected);
        }

        // 共用：儲存圖片到 wwwroot/Pictures，回傳相對路徑「/Pictures/xxx」
        private async Task<string?> SavePhotoAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0) return null;

            var folder = Path.Combine(_env.WebRootPath, "Pictures");
            Directory.CreateDirectory(folder);

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(folder, fileName);

            using (var fs = System.IO.File.Create(fullPath))
            {
                await file.CopyToAsync(fs);
            }
            return $"/Pictures/{fileName}";
        }

        // GET: /Admin/Products
        public async Task<IActionResult> Index(string? q, int? categoryId, string sort = "created_desc", int page = 1, int pageSize = 10)
        {
            var qry = _context.Product
                              .Include(p => p.CategoryInfo)
                              .AsNoTracking()
                              .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var key = q.Trim();
                qry = qry.Where(p => p.ProductName.Contains(key) ||
                                     p.ProductCode.Contains(key) ||
                                     p.Description.Contains(key));
            }

            if (categoryId.HasValue)
                qry = qry.Where(p => p.CategoryID == categoryId.Value);

            qry = sort switch
            {
                "name" => qry.OrderBy(p => p.ProductName),
                "name_desc" => qry.OrderByDescending(p => p.ProductName),
                "price" => qry.OrderBy(p => p.Price),
                "price_desc" => qry.OrderByDescending(p => p.Price),
                "created" => qry.OrderBy(p => p.CreatedDate),
                _ => qry.OrderByDescending(p => p.CreatedDate) // created_desc
            };

            var total = await qry.CountAsync();
            pageSize = Math.Clamp(pageSize, 5, 50);
            page = Math.Max(1, page);

            var items = await qry.Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();

            ViewBag.Q = q;
            ViewBag.CategoryId = categoryId;
            ViewBag.Sort = sort;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Total = total;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize);
            await SetCategoriesAsync(categoryId);

            return View(items);
        }

        // GET: /Admin/Products/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            var item = await _context.Product
                                     .Include(p => p.CategoryInfo)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(p => p.ProductID == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // GET: /Admin/Products/Create
        public async Task<IActionResult> Create()
        {
            await SetCategoriesAsync();
            return View(new ProductCreateVm());
        }

        // POST: /Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVm vm, IFormFile? PhotoFile)
        {
            if (!ModelState.IsValid)
            {
                await SetCategoriesAsync(vm.CategoryID);
                return View(vm);
            }

            // 檢查 ProductCode 唯一
            var codeExists = await _context.Product.AnyAsync(p => p.ProductCode == vm.ProductCode);
            if (codeExists)
            {
                ModelState.AddModelError(nameof(vm.ProductCode), "此商品代碼已存在。");
                await SetCategoriesAsync(vm.CategoryID);
                return View(vm);
            }

            string? photoPath = await SavePhotoAsync(PhotoFile);

            var entity = new Product
            {
                ProductID = Guid.NewGuid(),
                ProductCode = vm.ProductCode.Trim(),
                ProductName = vm.ProductName.Trim(),
                Description = vm.Description.Trim(),
                Photo = photoPath,              // 若未上傳，為 null
                Price = vm.Price,
                IsActive = vm.IsActive,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CategoryID = vm.CategoryID
            };

            _context.Product.Add(entity);
            await _context.SaveChangesAsync();

            TempData["ok"] = "商品已新增。";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/Products/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var item = await _context.Product.FindAsync(id);
            if (item == null) return NotFound();

            var vm = new ProductEditVm
            {
                ProductID = item.ProductID,
                ProductCode = item.ProductCode,
                ProductName = item.ProductName,
                Description = item.Description,
                Photo = item.Photo,
                Price = item.Price,
                IsActive = item.IsActive,
                CategoryID = item.CategoryID
            };

            await SetCategoriesAsync(item.CategoryID);
            return View(vm);
        }

        // POST: /Admin/Products/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductEditVm vm, IFormFile? NewPhotoFile)
        {
            if (id != vm.ProductID) return NotFound();

            if (!ModelState.IsValid)
            {
                await SetCategoriesAsync(vm.CategoryID);
                return View(vm);
            }

            var item = await _context.Product.FirstOrDefaultAsync(p => p.ProductID == id);
            if (item == null) return NotFound();

            // 檢查 ProductCode 唯一（排除自己）
            var codeExists = await _context.Product.AnyAsync(p => p.ProductCode == vm.ProductCode && p.ProductID != id);
            if (codeExists)
            {
                ModelState.AddModelError(nameof(vm.ProductCode), "此商品代碼已存在。");
                await SetCategoriesAsync(vm.CategoryID);
                return View(vm);
            }

            item.ProductCode = vm.ProductCode.Trim();
            item.ProductName = vm.ProductName.Trim();
            item.Description = vm.Description.Trim();
            item.Price = vm.Price;
            item.IsActive = vm.IsActive;
            item.CategoryID = vm.CategoryID;
            item.UpdatedDate = DateTime.Now;

            // 若有新圖，覆蓋 Photo
            var newPath = await SavePhotoAsync(NewPhotoFile);
            if (!string.IsNullOrWhiteSpace(newPath))
                item.Photo = newPath;

            await _context.SaveChangesAsync();
            TempData["ok"] = "商品已更新。";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/Products/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _context.Product
                                     .Include(p => p.CategoryInfo)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(p => p.ProductID == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: /Admin/Products/DeleteConfirmed/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var item = await _context.Product.FindAsync(id);
            if (item != null)
            {
                _context.Product.Remove(item);
                await _context.SaveChangesAsync();
            }
            TempData["ok"] = "商品已刪除。";
            return RedirectToAction(nameof(Index));
        }
    }
}
