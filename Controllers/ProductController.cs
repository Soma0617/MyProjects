using System.IO;
using FinalExamProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExamProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly GoShopContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(GoShopContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // 共用：儲存上傳圖片到 wwwroot/Pictures，回傳「檔名」
        private async Task<string?> SavePhotoAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0) return null;

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            if (!allowed.Contains(ext))
            {
                ModelState.AddModelError("PhotoFile", "僅支援 jpg、jpeg、png、gif、webp");
                return null;
            }

            var fileName = $"{Guid.NewGuid():N}{ext}";
            var saveDir = Path.Combine(_env.WebRootPath, "Pictures");
            Directory.CreateDirectory(saveDir);
            var savePath = Path.Combine(saveDir, fileName);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        // GET: Product/Index
        public async Task<IActionResult> Index([FromQuery] ProductQueryVm query)
        {
            var q = _context.Product
                .Include(p => p.CategoryInfo)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.Q))
            {
                var key = query.Q.Trim();
                q = q.Where(p =>
                    p.ProductName.Contains(key) ||
                    p.ProductCode.Contains(key) ||
                    p.Description.Contains(key));
            }

            if (query.CategoryID.HasValue)
                q = q.Where(p => p.CategoryID == query.CategoryID.Value);

            if (query.IsActive.HasValue)
                q = q.Where(p => p.IsActive == query.IsActive.Value);

            q = query.Sort switch
            {
                "name" => q.OrderBy(p => p.ProductName),
                "name_desc" => q.OrderByDescending(p => p.ProductName),
                "price" => q.OrderBy(p => p.Price),
                "price_desc" => q.OrderByDescending(p => p.Price),
                "created" => q.OrderBy(p => p.CreatedDate),
                _ => q.OrderByDescending(p => p.CreatedDate)
            };

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
                .AsNoTracking()
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

        // POST: Product/Create（已刪除手動檔名邏輯）
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Category.AsNoTracking().ToList();
                return View(vm);
            }

            // 商品代碼不可重複
            var exists = await _context.Product.AnyAsync(p => p.ProductCode == vm.ProductCode);
            if (exists)
            {
                ModelState.AddModelError(nameof(vm.ProductCode), "商品代碼已存在");
                ViewBag.Categories = _context.Category.AsNoTracking().ToList();
                return View(vm);
            }

            // 只接受上傳檔案（可選：你也可以改成必填）
            string? photoName = null;
            if (vm.PhotoFile != null)
            {
                photoName = await SavePhotoAsync(vm.PhotoFile);
                if (!ModelState.IsValid)
                {
                    ViewBag.Categories = _context.Category.AsNoTracking().ToList();
                    return View(vm);
                }
            }

            var entity = new Product
            {
                ProductID = Guid.NewGuid(),
                ProductCode = vm.ProductCode.Trim(),
                ProductName = vm.ProductName.Trim(),
                Description = vm.Description.Trim(),
                Photo = photoName, // 只來自上傳
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
                Photo = product.Photo,           // 顯示用（預覽）
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

            // 商品代碼不可與他人重複
            var codeExists = await _context.Product.AnyAsync(p => p.ProductCode == vm.ProductCode && p.ProductID != id);
            if (codeExists)
            {
                ModelState.AddModelError(nameof(vm.ProductCode), "商品代碼已存在");
                ViewBag.Categories = _context.Category.AsNoTracking().ToList();
                return View(vm);
            }

            // 有上傳新圖片才更新，否則保留舊圖
            if (vm.PhotoFile != null)
            {
                var newName = await SavePhotoAsync(vm.PhotoFile);
                if (!ModelState.IsValid)
                {
                    ViewBag.Categories = _context.Category.AsNoTracking().ToList();
                    return View(vm);
                }

                // 刪除舊檔（可選）
                if (!string.IsNullOrWhiteSpace(product.Photo))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, "Pictures", product.Photo);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                product.Photo = newName;
            }

            product.ProductCode = vm.ProductCode.Trim();
            product.ProductName = vm.ProductName.Trim();
            product.Description = vm.Description.Trim();
            product.Price = vm.Price;
            product.IsActive = vm.IsActive;
            product.CategoryID = vm.CategoryID;
            product.UpdatedDate = DateTime.Now;

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
                .AsNoTracking()
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
                if (!string.IsNullOrWhiteSpace(product.Photo))
                {
                    var path = Path.Combine(_env.WebRootPath, "Pictures", product.Photo);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }

                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
            }

            TempData["ok"] = "刪除成功";
            return RedirectToAction(nameof(Index));
        }
    }
}
