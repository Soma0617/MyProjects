using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalExamProject.Models;

namespace FinalExamProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly GoShopContext _context;

        public AdminController(GoShopContext context)
        {
            _context = context;
        }

        // GET: Admin
        public async Task<IActionResult> Index([FromQuery] AdminQueryVm query)
        {
            var q = _context.Admin.AsNoTracking();

            // 關鍵字搜尋
            if (!string.IsNullOrWhiteSpace(query.Q))
            {
                var key = query.Q.Trim();
                q = q.Where(a => a.UserName.Contains(key) || a.Email.Contains(key));
            }

            // 角色篩選
            if (!string.IsNullOrWhiteSpace(query.Role))
            {
                q = q.Where(a => a.Role == query.Role);
            }

            // 啟用狀態篩選
            if (query.IsActive.HasValue)
            {
                q = q.Where(a => a.IsActive == query.IsActive.Value);
            }

            // 排序
            q = query.Sort switch
            {
                "name" => q.OrderBy(a => a.UserName),
                "name_desc" => q.OrderByDescending(a => a.UserName),
                "email" => q.OrderBy(a => a.Email),
                "email_desc" => q.OrderByDescending(a => a.Email),
                "created" => q.OrderBy(a => a.CreatedDate),
                _ => q.OrderByDescending(a => a.CreatedDate) // 預設
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

            return View(items);
        }

        // GET: Admin/Create
        public IActionResult Create() => View(new AdminCreateVm());

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var entity = new Admin
            {
                AdminID = Guid.NewGuid(),
                UserName = vm.UserName.Trim(),
                Email = vm.Email.Trim(),
                PasswordHash = vm.PasswordHash,  // ⚠ 目前直接存字串，未來可改成 Hash
                Role = vm.Role.Trim(),
                IsActive = vm.IsActive,
                CreatedDate = DateTime.Now
            };

            _context.Admin.Add(entity);
            await _context.SaveChangesAsync();
            TempData["ok"] = "建立成功";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var admin = await _context.Admin.FindAsync(id);
            if (admin == null) return NotFound();

            var vm = new AdminEditVm
            {
                AdminID = admin.AdminID,
                UserName = admin.UserName,
                Email = admin.Email,
                Role = admin.Role,
                IsActive = admin.IsActive
            };

            return View(vm);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, AdminEditVm vm)
        {
            if (id != vm.AdminID) return NotFound();
            if (!ModelState.IsValid) return View(vm);

            var admin = await _context.Admin.FirstOrDefaultAsync(a => a.AdminID == id);
            if (admin == null) return NotFound();

            admin.UserName = vm.UserName.Trim();
            admin.Email = vm.Email.Trim();
            admin.Role = vm.Role.Trim();
            admin.IsActive = vm.IsActive;

            try
            {
                await _context.SaveChangesAsync();
                TempData["ok"] = "已儲存變更";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Admin.Any(e => e.AdminID == vm.AdminID)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var admin = await _context.Admin
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AdminID == id);

            if (admin == null) return NotFound();

            return View(admin);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var admin = await _context.Admin
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AdminID == id);

            if (admin == null) return NotFound();

            return View(admin);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var admin = await _context.Admin.FindAsync(id);
            if (admin != null)
            {
                _context.Admin.Remove(admin);
                await _context.SaveChangesAsync();
                TempData["ok"] = "已刪除";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(Guid id)
        {
            return _context.Admin.Any(e => e.AdminID == id);
        }
    }
}
