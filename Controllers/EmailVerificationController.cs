using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalExamProject.Models;

namespace FinalExamProject.Controllers
{
    public class EmailVerificationController : Controller
    {
        private readonly GoShopContext _context;

        public EmailVerificationController(GoShopContext context)
        {
            _context = context;
        }

        // GET: EmailVerification
        public async Task<IActionResult> Index([FromQuery] EmailVerificationQueryVm query)
        {
            var q = _context.EmailVerification
                            .Include(e => e.MemberInfo) // 使用你的導航屬性名稱
                            .AsNoTracking()
                            .AsQueryable();

            // 關鍵字（驗證碼 / 會員姓名 / 會員 Email）
            if (!string.IsNullOrWhiteSpace(query.Q))
            {
                var key = query.Q.Trim();
                q = q.Where(e =>
                    e.VerificationCode.Contains(key) ||
                    (e.MemberInfo != null &&
                       (e.MemberInfo.Name.Contains(key) || e.MemberInfo.Email.Contains(key))));
            }

            // 是否已驗證
            if (query.IsVerified.HasValue)
            {
                q = q.Where(e => e.IsVerified == query.IsVerified.Value);
            }

            // 到期區間
            if (query.ExpireFrom.HasValue)
            {
                var from = query.ExpireFrom.Value.Date;
                q = q.Where(e => e.ExpirationTime >= from);
            }
            if (query.ExpireTo.HasValue)
            {
                var to = query.ExpireTo.Value.Date.AddDays(1).AddTicks(-1);
                q = q.Where(e => e.ExpirationTime <= to);
            }

            // 排序
            q = query.Sort switch
            {
                "created" => q.OrderBy(e => e.CreatedDate),
                "expire" => q.OrderBy(e => e.ExpirationTime),
                "expire_desc" => q.OrderByDescending(e => e.ExpirationTime),
                "status" => q.OrderBy(e => e.IsVerified).ThenByDescending(e => e.CreatedDate),
                "status_desc" => q.OrderByDescending(e => e.IsVerified).ThenByDescending(e => e.CreatedDate),
                _ => q.OrderByDescending(e => e.CreatedDate) // created_desc (預設)
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

        // GET: EmailVerification/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var item = await _context.EmailVerification
                                     .Include(e => e.MemberInfo)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(m => m.VerificationID == id);
            if (item == null) return NotFound();

            return View(item);
        }

        // GET: EmailVerification/Create
        public IActionResult Create()
        {
            ViewBag.Members = _context.Member.AsNoTracking().ToList();
            return View(new EmailVerificationCreateVm());
        }

        // POST: EmailVerification/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmailVerificationCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Members = _context.Member.AsNoTracking().ToList();
                return View(vm);
            }

            var entity = new EmailVerification
            {
                VerificationID = Guid.NewGuid(),
                MemberID = vm.MemberID,
                VerificationCode = vm.VerificationCode.Trim(),
                ExpirationTime = vm.ExpirationTime,
                IsVerified = vm.IsVerified,
                CreatedDate = DateTime.Now
            };

            _context.EmailVerification.Add(entity);
            await _context.SaveChangesAsync();
            TempData["ok"] = "建立成功";
            return RedirectToAction(nameof(Index));
        }

        // GET: EmailVerification/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var item = await _context.EmailVerification.FindAsync(id);
            if (item == null) return NotFound();

            var vm = new EmailVerificationEditVm
            {
                VerificationID = item.VerificationID,
                MemberID = item.MemberID,
                VerificationCode = item.VerificationCode,
                ExpirationTime = item.ExpirationTime,
                IsVerified = item.IsVerified
            };

            ViewBag.Members = _context.Member.AsNoTracking().ToList();
            return View(vm);
        }

        // POST: EmailVerification/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EmailVerificationEditVm vm)
        {
            if (id != vm.VerificationID) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.Members = _context.Member.AsNoTracking().ToList();
                return View(vm);
            }

            var item = await _context.EmailVerification.FirstOrDefaultAsync(e => e.VerificationID == id);
            if (item == null) return NotFound();

            item.MemberID = vm.MemberID;
            item.VerificationCode = vm.VerificationCode.Trim();
            item.ExpirationTime = vm.ExpirationTime;
            item.IsVerified = vm.IsVerified;

            try
            {
                await _context.SaveChangesAsync();
                TempData["ok"] = "已儲存變更";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.EmailVerification.AnyAsync(e => e.VerificationID == id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: EmailVerification/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var item = await _context.EmailVerification
                                     .Include(e => e.MemberInfo)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(m => m.VerificationID == id);
            if (item == null) return NotFound();

            return View(item);
        }

        // POST: EmailVerification/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var item = await _context.EmailVerification.FindAsync(id);
            if (item != null)
            {
                _context.EmailVerification.Remove(item);
                await _context.SaveChangesAsync();
                TempData["ok"] = "已刪除";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
